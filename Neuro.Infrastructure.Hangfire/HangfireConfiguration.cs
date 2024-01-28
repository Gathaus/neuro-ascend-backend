using System.Linq.Expressions;
using System.Reflection;
using Hangfire;
using Hangfire.PostgreSql;
using Hangfire.States;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neuro.Application.Attributes;
using Neuro.Application.BackgroundJobs;
using Neuro.Application.Base.BackgroundJobs;
using Neuro.Infrastructure.Hangfire.Filters;

namespace Neuro.Infrastructure.Hangfire;

public class HangfireActivator : JobActivator
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HangfireActivator(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public override JobActivatorScope BeginScope(JobActivatorContext context)
    {
        return new HangfireScope(_serviceScopeFactory.CreateScope());
    }

    private class HangfireScope : JobActivatorScope
    {
        private readonly IServiceScope _serviceScope;

        public HangfireScope(IServiceScope serviceScope)
        {
            _serviceScope = serviceScope;
        }

        public override object Resolve(Type jobType)
        {
            var service = _serviceScope.ServiceProvider.GetService(jobType);
            if (service != null)
            {
                return service;
            }
            
            return ActivatorUtilities.CreateInstance(_serviceScope.ServiceProvider, jobType);
        }

        public override void DisposeScope()
        {
            _serviceScope.Dispose();
        }
    }
}


public static class HangfireConfiguration
{
    public static void AddHangfireServices(this IServiceCollection services, string connectionString)
    {
        services.AddHangfire(config => config
            .UsePostgreSqlStorage(connectionString));

        // HangfireActivator için IServiceScopeFactory örneği alın ve yapılandırın
        services.AddSingleton<JobActivator, HangfireActivator>(provider => 
            new HangfireActivator(provider.GetRequiredService<IServiceScopeFactory>())
        );

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var uniqueServerName = $"{Environment.MachineName}-{environmentName}";

        services.AddHangfireServer(x =>
        {
            x.ServerName = uniqueServerName;
            x.WorkerCount = 1;
            x.Queues = new[] { "default", "critical" };
            x.ServerTimeout = TimeSpan.FromMinutes(0.5); 
            // Yapılandırılmış HangfireActivator kullanın
            x.Activator = new HangfireActivator(services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>());
        });

        ConfigureJobFilters();
    }

    public static void RegisterAllRecurringJobs(Assembly jobAssembly, IServiceProvider serviceProvider)
    {
        var jobTypes = jobAssembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRecurringJob)));

        foreach (var jobType in jobTypes)
        {
            var executeMethod = jobType.GetMethod("Execute");
            var attribute = executeMethod.GetCustomAttribute<RecurringJobAttribute>();

            if (attribute != null)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var jobInstance = ActivatorUtilities.CreateInstance(scope.ServiceProvider, jobType);
                    var jobExpression = Expression.Lambda<Func<Task>>(
                        Expression.Call(Expression.Constant(jobInstance), executeMethod));

                    RecurringJob.AddOrUpdate(attribute.JobId, jobExpression, attribute.CronExpression);
                }
            }
        }
    }




    
    public static void RestartProcessingJobsBeforeStartingServer(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var timeThreshold = DateTime.UtcNow.AddHours(-1);

            var monitoringApi = JobStorage.Current.GetMonitoringApi();
            var processingJobs = monitoringApi.ProcessingJobs(0, int.MaxValue);

            foreach (var job in processingJobs)
            {
                var jobDetails = monitoringApi.JobDetails(job.Key);
                var jobCreatedAt = jobDetails.CreatedAt;

                if (jobCreatedAt > timeThreshold)
                {
                    var jobData = jobDetails.History.FirstOrDefault(h => h.StateName == ProcessingState.StateName);

                    if (jobData != null)
                    {
                        // Yeniden çalıştırılacak işi sıraya ekleyin
                        BackgroundJob.Requeue(job.Key);
                    }
                }
            }
        }
    }

    
    public static void ConfigureQueues(this IApplicationBuilder app)
    {
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() }
        });
    }
    public static void ConfigureJobFilters()
    {
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 5 });
        GlobalJobFilters.Filters.Add(new HangfireExceptionFilter());
    }
    
    public static IApplicationBuilder UseCustomHangfireDashboard(this IApplicationBuilder app, IServiceProvider serviceProvider)
    {
        
        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            Authorization = new[] { new HangfireAuthorizationFilter() },  
            StatsPollingInterval = 60000 // 1 minute
        });

        var jobAssembly = JobAssemblyProvider.GetJobAssembly();
        RegisterAllRecurringJobs(jobAssembly, serviceProvider);

        return app;
    }
}

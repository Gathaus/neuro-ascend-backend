using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neuro.Domain.Logging;
using Neuro.Infrastructure.Contexts;
using Neuro.Infrastructure.Excel;
using Neuro.Infrastructure.Logging;
using Neuro.Infrastructure.MessageBus.Consumers;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;

namespace Neuro.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IMemberContext, MemberContext>();
        services.AddScoped<SampleMessageConsumer>();
        services.AddScoped<IExcelService, ExcelService>();

    }
    
    public static IServiceCollection AddRemLogger(this IServiceCollection services, IConfigurationSection nlogConfigSection)
    {
        services.AddScoped<INeuroLogger, NeuroLogger>();
        LogManager.Configuration = new NLogLoggingConfiguration(nlogConfigSection);
        LogManager.ThrowConfigExceptions = true;
        LogManager.ThrowExceptions = true;

        return services;
    }

    public static IHostBuilder UseNeuroLogger(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseNLog();

        return hostBuilder;
    }
}
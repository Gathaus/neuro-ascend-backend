using Microsoft.Extensions.DependencyInjection;
using Neuro.Application.Base.Service;
using Neuro.Application.Managers.Abstract;
using Neuro.Application.Managers.Concrete;

namespace Neuro.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>(); 
        InjectSingleBusinessServices(services);
        InjectDoubleBusinessServices(services);
        InjectDoubleFilterServices(services);
        InjectDoubleDynamicServices(services);
        services.AddSingleton<INotificationManager, NotificationManager>();


        return services;
    }

    private static void InjectDoubleDynamicServices(IServiceCollection services)
    {
        // İki argümanlı olanlar için
        var dynamicServiceTypeDouble = typeof(IDynamicResultService<,>);
        var dynamicTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p =>
                p.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == dynamicServiceTypeDouble) &&
                !p.IsAbstract);

        foreach (var type in dynamicTypes)
        {
            services.AddScoped(type);
        }
    }

    private static void InjectDoubleFilterServices(IServiceCollection services)
    {
        // İki argümanlı olanlar için
        var filterServiceTypeDouble = typeof(IFilterService<,>);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p =>
                p.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == filterServiceTypeDouble) &&
                !p.IsAbstract);

        foreach (var type in types)
        {
            services.AddScoped(type);
        }
    }

    private static void InjectDoubleBusinessServices(IServiceCollection services)
    {
        // İki argümanlı olanlar için
        var businessServiceTypeDouble = typeof(IBusinessService<,>);
        var typesDouble = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p =>
                p.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == businessServiceTypeDouble) &&
                !p.IsAbstract);

        foreach (var type in typesDouble)
        {
            services.AddScoped(type);
        }
    }

    private static void InjectSingleBusinessServices(IServiceCollection services)
    {
        services.AddScoped<BaseBusinessService>();
        var businessServiceTypeSingle = typeof(IBusinessService<>);
        var typesSingle = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p =>
                p.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == businessServiceTypeSingle) &&
                !p.IsAbstract);

        foreach (var type in typesSingle)
        {
            services.AddScoped(type);
        }
    }
}
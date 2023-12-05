using Microsoft.Extensions.DependencyInjection;
using Neuro.Infrastructure.Excel;
using Neuro.Infrastructure.MessageBus.Consumers;
using POI.Application.Base.Excel;

namespace Neuro.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<SampleMessageConsumer>();
        services.AddScoped<IExcelService, ExcelService>();

    }
}
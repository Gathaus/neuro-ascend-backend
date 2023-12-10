using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neuro.Domain.Repositories;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Ef.Base;
using Neuro.Infrastructure.Ef.Contexts;

namespace Neuro.Infrastructure.Ef;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureEf(this IServiceCollection services, string connectionString,
        string logConnectionStr)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();


        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDbContext<NeuroLogDbContext>(options => options.UseNpgsql(logConnectionStr));
        
        return services;
    }
}
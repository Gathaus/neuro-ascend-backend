using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Neuro.Domain.Repositories;
using Neuro.Domain.UnitOfWork;
using Neuro.Infrastructure.Ef.Base;
using Neuro.Infrastructure.Ef.Contexts;

namespace Neuro.Infrastructure.Ef;

public static class DependencyInjection
{
    public static void AddInfrastructureEf(this IServiceCollection services, string connectionString)
    {
        
        if (string.IsNullOrEmpty(connectionString)) 
            return;

        services.AddScoped(typeof(IRepository<,>), typeof(BaseRepository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
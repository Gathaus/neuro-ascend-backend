using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;

namespace Neuro.Infrastructure.Ef.Contexts
{
    public class NeuroLogDbContext : DbContext
    {
        public NeuroLogDbContext(DbContextOptions<NeuroLogDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        }
    }
}

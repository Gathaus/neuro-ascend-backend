using Microsoft.EntityFrameworkCore;

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

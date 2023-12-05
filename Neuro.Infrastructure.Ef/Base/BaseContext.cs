using Microsoft.EntityFrameworkCore;

namespace Neuro.Infrastructure.Ef.Base
{
    public abstract class BaseContext : DbContext
    {
        #region Constructors

        protected BaseContext(DbContextOptions options)
            : base(options)
        {
        }

        #endregion

        #region Methods

        // Soft delete filter will be applied to all entities that implement ISoftDeletableEntity
        // OnModelCreating

        public override int SaveChanges()
        {
            // Delete operation will be added
            // Audit info will be added
            // Domain events will be dispatched with outbox pattern
            var result = base.SaveChanges();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Delete operation will be added
            // Audit info will be added
            var result = await base.SaveChangesAsync(cancellationToken);
            //outbox pattern 
            return result;
        }

        #endregion

    }
}

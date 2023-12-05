using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;

namespace Neuro.Infrastructure.Ef.Contexts;


    public class ApplicationDbContext : DbContext
    {
        #region Constructors

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        #endregion

        #region DbSets
        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            ApplyConfigurationsFromAssembly(builder);
        }
        
        
        private void ApplyConfigurationsFromAssembly(ModelBuilder builder)
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            var configurations = currentAssembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i => i.IsGenericType
                                                       && i.GetGenericTypeDefinition() ==
                                                       typeof(IEntityTypeConfiguration<>)))
                .ToList();

            foreach (var configuration in configurations)
            {
                dynamic configurationInstance = Activator.CreateInstance(configuration)!;
                builder.ApplyConfiguration(configurationInstance);
            }
        }
        
        private void RegisterDbSets(ModelBuilder modelBuilder)
        {
            var dbSetProperties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var prop in dbSetProperties)
            {
                var entityType = prop.PropertyType.GetGenericArguments().First();
                modelBuilder.Entity(entityType);
            }
        }


        #endregion
    }
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Neuro.Domain.Entities;

namespace Neuro.Infrastructure.Ef.Contexts;


    public class ApplicationDbContext : IdentityDbContext
    {
        #region Constructors

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        #endregion

        #region DbSets
        
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<RecommendedRoutine> RecommendedRoutines { get; set; }
        public DbSet<FoodPage> FoodPages { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<MedicineUser> MedicineUsers { get; set; }
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
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<UserMood> UserMoods { get; set; }
        public DbSet<UserProgress> UserProgresses { get; set; }
        public DbSet<Medication> Medications { get; set; }
        public DbSet<UserMedicine> UserMedicines { get; set; }
        public DbSet<ActivityImageDescription> ActivityImageDescriptions { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<MedicationTime> MedicationTimes { get; set; }
        public DbSet<UserTarget> UserTargets { get; set; }
        public DbSet<TargetGroup> TargetGroups { get; set; }
        public DbSet<UserWaitlist> UserWaitlist { get; set; }
        #endregion

        #region Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var auditedEntityTypes = builder.Model.GetEntityTypes()
                .Where(t => t.ClrType.IsSubclassOf(typeof(AuditedBaseEntity<,>)));

            builder.Entity<User>()
                .HasMany(u => u.Diseases) // User sınıfında bir Diseases koleksiyonu olduğunu varsayıyorum
                .WithOne() // Disease sınıfında geri referans olmadığını varsayıyorum
                .OnDelete(DeleteBehavior.Cascade); // User silindiğinde ilişkili Diseases de silinir

            // User ve UserMedicine arasındaki ilişkiyi yapılandırma
            builder.Entity<User>()
                .HasMany(u => u.UserMedicines)
                .WithOne(um => um.User)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade); // User silindiğinde ilişkili UserMedicines de silinir

            
            
            foreach (var entityType in auditedEntityTypes)
            {
                builder.Entity(entityType.Name).Property("Timestamp").IsConcurrencyToken();
            }          
            
            var timeSpanConverter = new ValueConverter<TimeSpan, TimeSpan>(
                modelTime => modelTime,
                providerTime => new TimeSpan(providerTime.Hours, providerTime.Minutes, providerTime.Seconds));

            // MedicationTime entity'si için Time özelliğine ValueConverter'ı uygulama
            builder.Entity<MedicationTime>().Property(e => e.Time).HasConversion(timeSpanConverter);


            ApplyConfigurationsFromAssembly(builder);
            
            
            
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditedEntity auditedEntity && 
                    (entry.State == EntityState.Modified || entry.State == EntityState.Added))
                {
                    auditedEntity.UpdateTimestamp();
                }
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditedEntity auditedEntity && 
                    (entry.State == EntityState.Modified || entry.State == EntityState.Added))
                {
                    auditedEntity.UpdateTimestamp();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
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
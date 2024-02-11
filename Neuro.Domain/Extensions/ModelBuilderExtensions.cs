using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Neuro.Domain.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder UseTimeSpanToTimeConverter(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(TimeSpan) || property.ClrType == typeof(TimeSpan?))
                {
                    property.SetValueConverter(new TimeSpanToTimeConverter());
                }
            }
        }

        return modelBuilder;
    }
}

public class TimeSpanToTimeConverter : ValueConverter<TimeSpan, string>
{
    public TimeSpanToTimeConverter() 
        : base(
            v => v.ToString(),
            v => TimeSpan.Parse(v))
    {
    }
}
using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class MedicationTime : BaseEntity<int>
{
    public TimeSpan Time { get; set; }
    public DayOfWeek WeekDay { get; set; }
    
    
}
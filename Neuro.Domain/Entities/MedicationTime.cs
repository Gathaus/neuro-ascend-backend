namespace Neuro.Domain.Entities;

public class MedicationTime : BaseEntity<int>
{
    public TimeSpan Time { get; set; }
    public DayOfWeek WeekDay { get; set; }

    public bool IsTaken { get; set; }
    
    
}
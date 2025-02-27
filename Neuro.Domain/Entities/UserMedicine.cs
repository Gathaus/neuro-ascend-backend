namespace Neuro.Domain.Entities;

public class UserMedicine : AuditedBaseEntity<int,int>
{
    public int? UserId { get; set; }
    public int? MedicationId { get; set; }
    public string? Email { get; set; }
    
    public string Usage { get; set; } // HowToUse
    public int PillNumber { get; set; }

    public DateOnly BeginningDate { get; set; }
    public DateOnly EndDate { get; set; }
    
    public ICollection<MedicationTime> MedicationTimes { get; set; }
    

    public User User { get; set; }
    public Medication Medication { get; set; }
}
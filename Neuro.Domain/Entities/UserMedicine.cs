using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class UserMedicine : AuditedBaseEntity<int,int>
{
    public int? UserId { get; set; }
    public int? MedicationId { get; set; }
    public string? Email { get; set; }
    
    public ICollection<MedicationDay> Days { get; set; } 
    public ICollection<TimeOfDay> Times { get; set; } 

    public User User { get; set; }
    public Medication Medication { get; set; }
}
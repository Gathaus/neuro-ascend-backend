namespace Neuro.Domain.Entities;

public class MedicineUser : BaseEntity<int>
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public int WeekDay { get; set; }
}
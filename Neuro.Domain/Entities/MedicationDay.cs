namespace Neuro.Domain.Entities;

public class MedicationDay : BaseEntity<int>
{
    public string? Email { get; set; }

    public DayOfWeek DayOfWeek { get; set; }
    public int UserMedicineId { get; set; }
    public UserMedicine UserMedicine { get; set; }
}
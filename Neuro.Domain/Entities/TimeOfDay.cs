namespace Neuro.Domain.Entities;

public class TimeOfDay : BaseEntity<int>
{
    public TimeSpan Time { get; set; }
    public int UserMedicineId { get; set; }
    public UserMedicine UserMedicine { get; set; }
}
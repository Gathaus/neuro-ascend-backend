using Neuro.Domain.Entities;

public class TimeOfDay : BaseEntity<int>
{
    public DateTime Time { get; set;}

    public int UserMedicineId { get; set; }
    public UserMedicine UserMedicine { get; set; }
}
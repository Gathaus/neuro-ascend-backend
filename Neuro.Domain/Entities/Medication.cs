namespace Neuro.Domain.Entities;

public class Medication : BaseEntity<int>
{
    public string Name { get; set; }
    public ICollection<UserMedicine> UserMedicines { get; set; }

}
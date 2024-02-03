using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class User : BaseEntity<int>
{
    public string? Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public BloodTypeEnum? BloodType { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public AlzheimerStageEnum AlzheimerStage { get; set; }

    public bool HavePet { get; set; }
    public bool WantVirtualPet { get; set; }

    public string CountryCode { get; set; }
    public string CountryCallingCode { get; set; }
    public string MobileNumber { get; set; } // PhoneNumber ile aynı veri
    public string? FirebaseToken { get; set; }

    public string? TimeZone { get; set; }
    
    public ICollection<Disease> Diseases { get; set; }
    public ICollection<UserMedicine> UserMedicines { get; set; }



}

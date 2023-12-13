using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class User : BaseEntity<int>
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public BloodTypeEnum BloodType { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string TempImageName { get; set; }
    public string? ImageUrl { get; set; }
    public string Disease { get; set; }
    public AlzheimerStageEnum AlzheimerStage { get; set; }
    public string View { get; set; }

    public string  ReminderTimeStr { get; set; }
    public DateTimeOffset ReminderTime { get; set; }
    public string HowToUse { get; set; }
    public DateOnly BeginningDate { get; set; }
    public DateOnly EndDate { get; set; }

    public List<WeekDaysEnum> MedicationDays { get; set; }
    public bool HavePet { get; set; }
    public bool WantVirtualPet { get; set; }
    
}
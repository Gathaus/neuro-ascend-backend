using Neuro.Domain.Entities;
using Neuro.Domain.Entities.Enums;

namespace Neuro.Api.Models;

public class RegisterModel
{
    public string? Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public BloodTypeEnum BloodType { get; set; }
    public int Age { get; set; }
    public string Address { get; set; }
    public string? ImageUrl { get; set; }
    public string DiseaseTerm { get; set; }
    public AlzheimerStageEnum DiseaseLevel { get; set; }
    public string View { get; set; }
    public DateTimeOffset ReminderTime { get; set; }
    public DateOnly BeginningDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool HavePet { get; set; }
    public bool WantVirtualPet { get; set; }
    public string CountryCode { get; set; }
    public string CountryCallingCode { get; set; }
    public string MobileNumber { get; set; } // PhoneNumber ile aynı veri
    public string Amount { get; set; } // Veri tipi kontrol edilmeli
    public int BeginDay { get; set; } // Veri tipi değişti
    public string BeginMonth { get; set; }
    public int EndDay { get; set; } // Veri tipi değişti
    public string EndMonth { get; set; }
    public List<WeekDaysEnum> SelectedDays { get; set; } // Veri tipi ve isim değişti (MedicationDays)
    public string Time { get; set; } // ReminderTimeStr ile aynı veri
    public string Usage { get; set; } // HowToUse ile aynı veri
    public string? FirebaseToken { get; set; }
}

public class LoginModel
{
    public string Email { get; set; }
    public string? Password { get; set; }
    public string FirebaseToken { get; set; }
}

public class UserRoleModel
{
    public string UserName { get; set; }
    public string RoleName { get; set; }
}

public class UserInfo
{
    public string Name { get; set; }
    public string Email { get; set; }
}


public class UserClaimModel
{
    public string UserName { get; set; }
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}

public class UpdateUserModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
}

public class ResetPasswordModel
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string NewPassword { get; set; }
}

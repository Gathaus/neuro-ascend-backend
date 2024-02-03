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
    public AlzheimerStageEnum AlzheimerStage { get; set; }
    public bool HasPet { get; set; }
    public bool WantsVirtualPet { get; set; }
    public string CountryCode { get; set; }
    public string CountryCallingCode { get; set; }
    public string MobileNumber { get; set; }
    public string? FirebaseToken { get; set; }
    public List<MedicationModel> Medications { get; set; } = new List<MedicationModel>();
    public List<string> Diseases { get; set; } = new List<string>();
}

public class MedicationModel
{
    public string Name { get; set; }
    public string Usage { get; set; }
    public int PillNumber { get; set; }
    public DateOnly BeginningDate { get; set; }
    public DateOnly EndDate { get; set; }
    public List<MedicationDayModel> MedicationDays { get; set; } = new List<MedicationDayModel>();
    public List<TimeOfDayModel> MedicationTimes { get; set; } = new List<TimeOfDayModel>();
}

public class MedicationDayModel
{
    public DayOfWeek DayOfWeek { get; set; }
}

public class TimeOfDayModel
{
    public DateTime Time { get; set; }
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

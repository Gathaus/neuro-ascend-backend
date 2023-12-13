using Neuro.Domain.Entities.Enums;

namespace Neuro.Api.Models;

public class RegisterModel
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

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
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

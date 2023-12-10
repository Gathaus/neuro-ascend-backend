namespace Neuro.Api.Models;

public class RegisterModel
{
    public string? Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
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

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Neuro.Domain.Entities;
using Neuro.Domain.Entities.Enums;
using Neuro.Domain.UnitOfWork;
using Newtonsoft.Json;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IConfiguration _config;
    private readonly IUnitOfWork _unitOfWork;

    public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration, IConfiguration config, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _config = config;
        _unitOfWork = unitOfWork;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterModel model)
    {
        var user = new IdentityUser {UserName = model.Email, Email = model.Email};
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User"); // Default role
            return Ok(new {Username = user.UserName});
        }

        return BadRequest(result.Errors);
    }

    // [HttpPost("addMedicine")]
    // public async Task<IActionResult> AddMedicine(MedicineModel model)
    // {
    //    TODO CHANGE THIS MEDICINE
    //     var data = await _unitOfWork.Repository<MedicineUser>().FindBy(x =>
    //             x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()) && x.WeekDay == model.WeekDay)
    //         .FirstOrDefaultAsync();
    //     if (data != null)
    //     {
    //         return BadRequest("You already added medicine.");
    //     }
    //    
    //     //TODO CHANGE THIS MEDICINE
    //     var usermedicine = new MedicineUser()
    //     {
    //         Email = model.Email,
    //         WeekDay = model.WeekDay,
    //     };
    //    
    //     var result = await _unitOfWork.Repository<MedicineUser>().InsertAsync(usermedicine);
    //     var rows = await _unitOfWork.SaveChangesAsync();
    //     if (rows > 0)
    //     {
    //         return Ok(new {IsSuccess = true});
    //     }
    //
    //     return BadRequest(new {IsSuccess = false});
    // }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var tokenString = GenerateTokenString(user);
            return Ok(new {Token = tokenString});
        }

        return Unauthorized();
    }

    [HttpPost("createrole")]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName)) return BadRequest("Role name is required");

        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    public class GoogleToken
    {
        public string IdToken { get; set; }
        public string? FirebaseToken { get; set; }
    }

    public class EmailModel
    {
        public string Email { get; set; }
        public string? FirebaseToken { get; set; }
    }
    
    public class GoogleTokenForRegister
    {
        public string IdToken { get; set; }
    }

    [HttpPost("signin-google")]
    public async Task<IActionResult> VerifyGoogleToken([FromBody] GoogleToken model)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);
        if (payload != null && !string.IsNullOrEmpty(payload.Email))
        {
            // var user = await _unitOfWork.Repository<User>().FindBy(x => x.Email.Equals(payload.Email,
            //     StringComparison.OrdinalIgnoreCase)).FirstOrDefaultAsync();

            var user = await _unitOfWork.Repository<User>()
                .FindBy(x => x.Email.ToLower().Trim().Equals(payload.Email.ToLower().Trim()))
                .FirstOrDefaultAsync();


            if (user != null)
            {
                if (model.FirebaseToken != null)
                {
                    user.FirebaseToken = model.FirebaseToken;
                    _unitOfWork.Repository<User>().Update(user);
                    await _unitOfWork.SaveChangesAsync();
                }

                var medicineDays = await _unitOfWork.Repository<MedicationDay>()
                    .FindBy(x => x.Email.ToLower().Trim().Equals(payload.Email.ToLower().Trim()))
                    .Select(x => x.DayOfWeek)
                    .ToListAsync();

                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(payload.Email.ToLower().Trim()))
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date).ToListAsync();
                var userMedicine = await _unitOfWork.Repository<UserMedicine>()
                    .FindBy(x => (x.Email.ToLower().Trim().Equals(payload.Email.ToLower().Trim()))
                                 && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date).ToListAsync();
                return Ok(new
                {
                    User = user, MedicineDays = medicineDays,
                    UserMood = userMood.FirstOrDefault()?.Mood.ToString() ?? "None",
                    IsSuccess = true
                });
            }

            return BadRequest(new {IsSuccess = false, Message = "User Not Found"});

            // var user = await _userManager.FindByNameAsync(payload?.Email ?? "");
            // if (user != null)
            // {
            //     var tokenString = GenerateTokenString(user);
            //     return Ok(new {IsSuccess = true, Token = tokenString});
            // }
        }

        return BadRequest("Invalid Google ID Token.");
    }

    [HttpPost("signin-email")]
    public async Task<IActionResult> SignInWithEmail([FromBody] EmailModel model)
    {
        if (!string.IsNullOrEmpty(model.Email))
        {
            var normalizedEmail = model.Email.ToLower().Trim();
            var user = await _unitOfWork.Repository<User>()
                .FindBy(x => x.Email.ToLower().Trim() == normalizedEmail)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                if (model.FirebaseToken != null)
                {
                    user.FirebaseToken = model.FirebaseToken;
                    _unitOfWork.Repository<User>().Update(user);
                    await _unitOfWork.SaveChangesAsync();
                }

                var medicineDays = await _unitOfWork.Repository<MedicationDay>()
                    .FindBy(x => x.User.Email.ToLower().Trim() == normalizedEmail)
                    .Select(x => x.DayOfWeek)
                    .ToListAsync();

                var userMood = await _unitOfWork.Repository<UserMood>()
                    .FindBy(x => x.User.Email.ToLower().Trim() == normalizedEmail && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date)
                    .ToListAsync();

                var userMedicine = await _unitOfWork.Repository<UserMedicine>()
                    .FindBy(x => x.User.Email.ToLower().Trim() == normalizedEmail && x.CreatedAt.Date == DateTimeOffset.UtcNow.Date)
                    .ToListAsync();

                return Ok(new
                {
                    User = user,
                    MedicineDays = medicineDays,
                    UserMood = userMood.FirstOrDefault()?.Mood.ToString() ?? "None",
                    IsSuccess = true
                });
            }

            return BadRequest(new { IsSuccess = false, Message = "User Not Found" });
        }

        return BadRequest("Invalid Email Address.");
    }
    
    
    [HttpPost("user-info-from-google")]
    public async Task<IActionResult> GetUserInfoFromGoogle([FromBody] GoogleTokenForRegister model)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);
        if (payload != null && !string.IsNullOrEmpty(payload.Email))
        {
            var user = await _unitOfWork.Repository<User>().FindBy(x => x.Email.Equals(payload.Email))
                .FirstOrDefaultAsync();
            if (user != null)
            {
                return BadRequest(new {IsSuccess = false, Message = "User already exists."});
            }

            return Ok(new
            {
                Email = payload.Email,
                IsSuccess = true
            });
        }

        return BadRequest("Invalid Google ID Token.");
    }

    [HttpPost("setUserMood")]
    public async Task<IActionResult> SetUserMood([FromBody] UserMoodRequest model)
    {
        if (model.UserId == null)
        {
            return BadRequest(new {IsSuccess = false});
        }

        var user = await _unitOfWork.Repository<User>()
            .FindBy(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
            .FirstOrDefaultAsync();
        if (user != null)
        {
            var userMood = new UserMood()
            {
                Email = model.Email,
                Mood = model.Mood,
                CreatedAt = DateTimeOffset.UtcNow
            };
            await _unitOfWork.Repository<UserMood>().InsertAsync(userMood);
            var rows = await _unitOfWork.SaveChangesAsync();
            if (rows > 0)
            {
                return Ok(new {IsSuccess = true});
            }

            return BadRequest(new {IsSuccess = false});
        }

        return BadRequest(new {IsSuccess = false});
    }

    // [HttpPost("setUserMedicine")]
    // public async Task<IActionResult> SetUserMedicine([FromBody] UserMedicineRequest model)
    // {
    //     if (model.UserId == null)
    //     {
    //         return BadRequest(new {IsSuccess = false});
    //     }
    //
    //     var user = await _unitOfWork.Repository<User>()
    //         .FindBy(x => x.Email.ToLower().Trim().Equals(model.Email.ToLower().Trim()))
    //         .FirstOrDefaultAsync();
    //     if (user != null)
    //     {
    //         var userMedicine = new UserMedicine()
    //         {
    //             Email = model.Email,
    //             IsTaken = model.IsTaken,
    //             CreatedAt = DateTimeOffset.UtcNow
    //         };
    //         await _unitOfWork.Repository<UserMedicine>().InsertAsync(userMedicine);
    //         var rows = await _unitOfWork.SaveChangesAsync();
    //         if (rows > 0)
    //         {
    //             return Ok(new {IsSuccess = true});
    //         }
    //
    //         return BadRequest(new {IsSuccess = false});
    //     }
    //
    //     return BadRequest(new {IsSuccess = false});
    // }


    [HttpPost("signin-google2")]
    public async Task<IActionResult> VerifyGoogleToken2([FromBody] GoogleToken model)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken);
        if (payload != null && !string.IsNullOrEmpty(payload.Email))
        {
            var normalizedEmail = payload.Email.ToLower();
            var user = await _unitOfWork.Repository<User>()
                .FindBy(x => x.Email.ToLower().Equals(normalizedEmail))
                .FirstOrDefaultAsync();


            if (user != null)
            {
                return Ok(new {IsSuccess = true});
            }

            return BadRequest(new {errorMessage = "User not found."});
        }

        return BadRequest(new {errorMessage = "Invalid Google ID Token."});
    }


    [HttpPost("deleterole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null) return NotFound("Role not found");

        var result = await _roleManager.DeleteAsync(role);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("assignrole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(UserRoleModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return NotFound("User not found");

        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("userhasrole")]
    [Authorize]
    public async Task<IActionResult> UserHasRole(UserRoleModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return NotFound("User not found");

        var hasRole = await _userManager.IsInRoleAsync(user, model.RoleName);
        if (hasRole)
        {
            return Ok($"User {model.UserName} is in role {model.RoleName}.");
        }

        return BadRequest($"User {model.UserName} is not in role {model.RoleName}.");
    }

    [HttpPost("addclaim")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddClaim(UserClaimModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return NotFound("User not found");

        var claim = new Claim(model.ClaimType, model.ClaimValue);
        var result = await _userManager.AddClaimAsync(user, claim);

        if (result.Succeeded)
        {
            return Ok($"Claim {model.ClaimType} added to user {model.UserName}.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("removeclaim")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveClaim(UserClaimModel model)
    {
        var user = await _userManager.FindByNameAsync
            (model.UserName);
        if (user == null) return NotFound("User not found");
        var claims = await _userManager.GetClaimsAsync(user);
        var claim = claims.FirstOrDefault(c => c.Type == model.ClaimType && c.Value == model.ClaimValue);
        if (claim == null) return NotFound("Claim not found");

        var result = await _userManager.RemoveClaimAsync(user, claim);
        if (result.Succeeded)
        {
            return Ok($"Claim {model.ClaimType} removed from user {model.UserName}.");
        }

        return BadRequest(result.Errors);
    }

    [HttpGet("userdetails")]
    [Authorize]
    public async Task<IActionResult> GetUserDetails(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        var claims = await _userManager.GetClaimsAsync(user);

        return Ok(new
        {
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles,
            Claims = claims.Select(c => new {c.Type, c.Value})
        });
    }

    [HttpPost("removerole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRoleFromUser(UserRoleModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return NotFound("User not found");

        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
        if (result.Succeeded)
        {
            return Ok($"User {model.UserName} removed from role {model.RoleName}.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPut("updateuser")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(UpdateUserModel model)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound("User not found");

        user.Email = model.Email;
        user.UserName = model.UserName;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok("User updated successfully.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("resetpassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return BadRequest("User not found");

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        if (result.Succeeded)
        {
            return Ok("Password reset successfully.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("adduserclaim")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddUserClaim(UserClaimModel model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return NotFound("User not found");

        var claim = new Claim(model.ClaimType, model.ClaimValue);
        var result = await _userManager.AddClaimAsync(user, claim);
        if (result.Succeeded)
        {
            return Ok($"Claim added to user {model.UserName}.");
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("enable2fa")]
    [Authorize]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null) return NotFound("User not found");

        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        // Bu tokenı kullanıcıya e-posta/SMS yoluyla gönderin

        return Ok("2FA token generated.");
    }

    [HttpPost("lockuser")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> LockUser(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound("User not found");

        var result = await _userManager.SetLockoutEnabledAsync(user, true);
        if (result.Succeeded)
        {
            return Ok($"User {userName} locked.");
        }

        return BadRequest(result.Errors);
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddHours(3),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateTokenString(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User"),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCred);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }

    private string GenerateTokenString(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, "User"),
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));

        var signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

        var securityToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            issuer: _config.GetSection("Jwt:Issuer").Value,
            audience: _config.GetSection("Jwt:Audience").Value,
            signingCredentials: signingCred);

        string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return tokenString;
    }
}
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Neuro.Api.Models;
using Neuro.Application.Base.Service;
using Newtonsoft.Json;

namespace Neuro.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AccountController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterModel model)
    {
        var user = new IdentityUser { UserName = model.Email, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User"); // Default role
            return Ok(new { Username = user.UserName });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var tokenString = GenerateJwtToken(user);
            return Ok(new { Token = tokenString });
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
    
    [HttpGet("signin-google")]
    public async Task<IActionResult> GoogleResponse(string code)
    {
        // 'code' parametresi Google tarafından gönderilen authorization code'dur.
        // Bu code'u kullanarak Google'dan access token ve refresh token alabilirsiniz.
        // Google API'sine istekte bulunacak HttpClient oluşturun
        var httpClient = new HttpClient();
    
        // Token isteği için gerekli verileri hazırlayın
        var requestData = new Dictionary<string, string>
        {
            { "code", code },
            { "client_id", _configuration["Google:ClientId"] },
            { "client_secret", _configuration["Google:ClientSecret"] },
            { "redirect_uri", "https://neuroascend.azurewebsites.net/signin-google" },
            { "grant_type", "authorization_code" }
        };
    
        // Google token endpoint'ine istek gönderin
        var response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(requestData));
        if (response.IsSuccessStatusCode)
        {
            // Google'dan gelen yanıtı okuyun
            var responseString = await response.Content.ReadAsStringAsync();
            var tokenData = JsonConvert.DeserializeObject<GoogleTokenResponse>(responseString);
    
            // Kullanıcının bilgilerini almak için token'ı kullanın ve sisteminize kaydedin
            // Burada kullanıcıyı sisteminize kaydetme ve kendi JWT'nizi oluşturma işlemlerinizi yapabilirsiniz
    
            // Kullanıcıya sisteminizin token'ını döndürün
            return Ok(new { Token = "Sizin Oluşturduğunuz JWT Token" });
        }
    
        // Bir hata oluşursa kullanıcıya bilgi döndürün
        return BadRequest("Google'dan token alınamadı.");
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
            Claims = claims.Select(c => new { c.Type, c.Value })
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
}
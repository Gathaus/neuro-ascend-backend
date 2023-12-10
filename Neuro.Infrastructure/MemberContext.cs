using Microsoft.AspNetCore.Http;
using Neuro.Domain.Entities.Enums;
using Neuro.Infrastructure.Constants;
using Neuro.Infrastructure.Contexts;
using Newtonsoft.Json;

namespace Neuro.Infrastructure
{
    public class MemberContext : IMemberContext
    {
        public MemberContext(IHttpContextAccessor httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
            {
                var roles = Enum.GetNames<UserType>();
                var claims = httpContextAccessor.HttpContext.User.Claims;
                if (Enum.TryParse(claims.SingleOrDefault(c => c.Type == ClaimTypes.Role && roles.Contains(c.Value))?.Value, out UserType role))
                    this.Role = role;

                this.FirstName = claims.Single(c => c.Type == ClaimTypes.FirstName).Value;
                this.SecondName = claims.SingleOrDefault(c => c.Type == ClaimTypes.SecondName)?.Value;
                this.LastName = claims.Single(c => c.Type == ClaimTypes.LastName).Value;
                this.Email = claims.Single(c => c.Type == ClaimTypes.Email).Value;
                this.Username = claims.Single(c => c.Type == ClaimTypes.Username).Value;
                this.IsSoUser = bool.Parse(claims.Single(c => c.Type == ClaimTypes.IsSoUser).Value);
                this.PrivateGuid = Guid.Parse(claims.Single(c => c.Type == ClaimTypes.PrivateGuid).Value);
                this.IsAnonym = false;
                this.UserId = int.Parse(claims.Single(c => c.Type == ClaimTypes.UserId).Value);

                this.Version = claims.SingleOrDefault(c => c.Type == ClaimTypes.MobileVersion)?.Value;
                this.DeviceId = claims.SingleOrDefault(c => c.Type == ClaimTypes.DeviceId)?.Value;
                this.Avatar = claims.SingleOrDefault(c => c.Type == ClaimTypes.Avatar)?.Value;
                this.Profiles = new Lazy<List<MemberProfile>>(() =>
                {
                    var profilesJson = claims.SingleOrDefault(c => c.Type == ClaimTypes.Profiles)?.Value;

                    if (profilesJson == null)
                        return new();

                    return JsonConvert.DeserializeObject<List<MemberProfile>>(profilesJson) ?? new();
                });
            }
            else
            {
                Profiles = new Lazy<List<MemberProfile>>(new List<MemberProfile>());
            }
        }

        public int? UserId { get; init; }
        public string? FirstName { get; init; }
        public string? SecondName { get; init; }
        public string? LastName { get; init; }
        public string? Fullname
        {
            get
            {
                return string.Join(' ', new[] { FirstName, SecondName, LastName }.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public string? Email { get; init; }
        public string? Username { get; init; }
        public string? PhoneNumber { get; init; }
        public string? DeviceId { get; init; }
        public string? Avatar { get; init; }
        public bool IsCompanyEmployee { get; init; }
        public Guid? PrivateGuid { get; init; }
        public UserType? Role { get; init; }
        public bool IsAnonym { get; init; } = true;
        public string? Version { get; init; }
        public bool IsSoUser { get; init; }
        public Lazy<List<MemberProfile>> Profiles { get; init; }

        IList<IMemberProfile> IMemberContext.Profiles => Profiles.Value.Cast<IMemberProfile>().ToList();
    }

    public class MemberProfile : IMemberProfile
    {
        public int CompanyId { get; set; }
        public bool IsSuperUser { get; set; }
        public Guid CompanyGuid { get; set; }
    }

}

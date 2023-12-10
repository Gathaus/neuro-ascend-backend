
using Neuro.Domain.Entities.Enums;

namespace Neuro.Infrastructure.Contexts
{
    public interface IMemberContext
    {
        public int? UserId { get; init; }
        public string? FirstName { get; init; }
        public string? SecondName { get; init; }
        public string? LastName { get; init; }
        public string? Fullname { get; }
        public string? Email { get; init; }
        public string? Username { get; init; }
        public string? PhoneNumber { get; init; }
        public string? Avatar { get; init; }
        public string? DeviceId { get; init; }
        public bool IsCompanyEmployee { get; init; }
        public Guid? PrivateGuid { get; init; }
        public UserType? Role { get; init; }
        public bool IsAnonym { get; init; }
        public bool IsSoUser { get; init; }
        public string? Version { get; init; }

        public IList<IMemberProfile> Profiles { get; }
    }

    public interface IMemberProfile
    {
        public int CompanyId { get;}
        public bool IsSuperUser { get; }
        public Guid CompanyGuid { get; }
    }
}
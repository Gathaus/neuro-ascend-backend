using Neuro.Domain.Entities.Enums;

namespace Neuro.Api.Models // Bunu domain içinde ayrı bir namespace yapmak tercih olabilir
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public AlzheimerStageEnum AlzheimerStage { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; } 
        public string? TimeZone { get; set; }
        public string? ImageUrl { get; set; }
        public string? FirebaseToken { get; set; }  
    }
}
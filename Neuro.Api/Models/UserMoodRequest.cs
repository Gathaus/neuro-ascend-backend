using Neuro.Domain.Entities.Enums;

namespace Neuro.Api.Models;

public class UserMoodRequest
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public MoodEnum Mood { get; set; }
    
}
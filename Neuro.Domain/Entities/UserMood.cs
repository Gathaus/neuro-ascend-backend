using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class UserMood : AuditedBaseEntity<int,int>
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public MoodEnum Mood { get; set; }
}
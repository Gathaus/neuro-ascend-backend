using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class UserMedicine : AuditedBaseEntity<int,int>
{
    public int? UserId { get; set; }
    public string? Email { get; set; }
    public bool IsTaken { get; set; }
}
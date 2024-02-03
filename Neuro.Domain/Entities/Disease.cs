namespace Neuro.Domain.Entities;

public class Disease : BaseEntity<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
}
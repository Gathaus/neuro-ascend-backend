namespace Neuro.Domain.Entities;

public class ActivityImageDescription : BaseEntity<int>
{
    public int ActivityId { get; set; }
    public int Order { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public Activity Activity { get; set; }
    
}

namespace Neuro.Domain.Entities;

public class UserTarget: BaseEntity<int>
{
    public int UserId { get; set; }
    public int TargetGroupId { get; set; }
    public short? MedicineTaken { get; set; }
    public short? MorningFoodTaken { get; set; }
    public short? EveningFoodTaken { get; set; }
    public short? ActivityDone { get; set; }
    public short? ExerciseDone { get; set; }
    public short? ArticleDone { get; set; }
    public DateTime CreatedDate { get; set; }
    
    public User User { get; set; }
    public TargetGroup TargetGroup { get; set; }
}
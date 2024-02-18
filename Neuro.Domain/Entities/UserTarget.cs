namespace Neuro.Domain.Entities;

public class UserTarget: BaseEntity<int>
{
    public int UserId { get; set; }
    public short MedicineTaken { get; set; }
    public short MecidineTarget { get; set; }
    
    public short MorningFoodTaken { get; set; }
    public short MorningFoodTarget { get; set; }
    
    public short ActivityDone { get; set; }
    public short ActivityTarget { get; set; }
    
    public short ExerciseDone { get; set; }
    public short ExerciseTarget { get; set; }
    
    public User User { get; set; }
}
namespace Neuro.Domain.Entities;

public class TargetGroup: BaseEntity<int>
{
    public short? MedicineTarget { get; set; }
    public short? MorningFoodTarget { get; set; }
    public short? EveningFoodTarget { get; set; }
    public short? ActivityTarget { get; set; }
    public short? ExerciseTarget { get; set; }
    public short? ArticleTarget { get; set; }
    
}
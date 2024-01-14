namespace Neuro.Api.Models;

public class UserProgressDto
{
    public int? MorningLastFoodId { get; set; }

    public int? EveningLastFoodId { get; set; }

    public int? LastExerciseId { get; set; }

    public int? LastActivityId { get; set; }

    public int? LastArticleId { get; set; }
}
using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class RecommendedRoutine : BaseEntity<int>
{
    public string? ImagePath { get; set; }
    public string Title { get; set; }
    public int? Calories { get; set; }
    public string? DescriptionImagePath { get; set; }
    public string? Description { get; set; }
    public RecommendationTypeEnum RecommendationType { get; set; }

    public Activity? Activity { get; set; }
    public int? ActivityId { get; set; }
    public Exercise? Exercise { get; set; }
    public int? ExerciseId { get; set; }
    public FoodPage? Food { get; set; }
    public int? FoodId { get; set; }
    public Article? Article { get; set; }
    public int? ArticleId { get; set; }
}


using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class RecommendedRoutine : BaseEntity<int>
{
    public string ImagePath { get; set; }
    public string Title { get; set; }
    public int Calories { get; set; }
    public string DescriptionImagePath { get; set; }
    public string Description { get; set; }
    public RecommendationTypeEnum RecommendationType { get; set; }
}
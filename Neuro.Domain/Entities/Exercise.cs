using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class Exercise : BaseEntity<int>
{
    public string Name { get; set; }
    public string AverageDuration { get; set; }
    public string? GifPath { get; set; }
    public List<string> Areas { get; set; }
    public string AreasImagePath { get; set; }
    public string Category { get; set; }
    public List<AlzheimerStageEnum> EligibleAlzheimerDegrees { get; set; }
    public List<string> InappropriateDiseases { get; set; }
    public List<string> InappropriateDrugs { get; set; }
    public List<string> Warnings { get; set; }
    public List<string> RequiredTools { get; set; }
    public List<string> ExerciseSteps { get; set; }
    public string NumberRepetitionsSets { get; set; }
    public List<string> Suggestions { get; set; }
    public List<string> Benefits { get; set; }
    public List<string> ExerciseImageUrls { get; set; }
}


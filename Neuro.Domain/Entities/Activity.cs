using System.ComponentModel.DataAnnotations;
using Neuro.Domain.Entities.Enums;

namespace Neuro.Domain.Entities;

public class Activity : BaseEntity<int>
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }

    public string? ImagePath { get; set; }

    public List<AlzheimerStageEnum> AlzheimerLevel { get; set; }

    public string RiskGroup { get; set; }

    public List<string> Materials { get; set; }

  
    public List<string> Steps { get; set; }


    public string Description { get; set; }

    public List<string> Warnings { get; set; }

    public List<string> Suggestions { get; set; }

    public List<string> Benefits { get; set; }
    
    public List<string> ActivityImagePaths { get; set; }
}


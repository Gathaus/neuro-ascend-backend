using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neuro.Domain.Entities;

public class UserProgress : BaseEntity<int>
{
    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Food")]
    public int? LastFoodId { get; set; }

    [ForeignKey("Exercise")]
    public int? LastExerciseId { get; set; }

    [ForeignKey("Activity")]
    public int? LastActivityId { get; set; }

    [ForeignKey("Article")]
    public int? LastArticleId { get; set; }

    public virtual User User { get; set; }
    public virtual FoodPage Food { get; set; }
    public virtual Exercise Exercise { get; set; }
    public virtual Activity Activity { get; set; }
    public virtual Article Article { get; set; }
}

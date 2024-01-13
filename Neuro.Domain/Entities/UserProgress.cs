using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Neuro.Domain.Entities
{
    public class UserProgress : BaseEntity<int>
    {
        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("MorningLastFood")]
        public int? MorningLastFoodId { get; set; }

        [ForeignKey("EveningLastFood")]
        public int? EveningLastFoodId { get; set; }

        [ForeignKey("Exercise")]
        public int? LastExerciseId { get; set; }

        [ForeignKey("Activity")]
        public int? LastActivityId { get; set; }

        [ForeignKey("Article")]
        public int? LastArticleId { get; set; }

        public virtual User User { get; set; }
        public virtual FoodPage MorningLastFood { get; set; }
        public virtual FoodPage EveningLastFood { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Article Article { get; set; }
    }
}
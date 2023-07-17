using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Scores;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HarzRollerz.MVC.Models.SingingFeatures
{
    public class EvaluatedFeature
    {
        [Key]
        public int ID { get; private set; }

        [ForeignKey(nameof(Competition))]
        public int CompetitionID { get; private set; }

        [ForeignKey(nameof(SingingFeature))]
        public int SingingFeatureID { get; private set; }

        [Required, StringLength(32)]
        public string Name { get; set; } = string.Empty;

        [StringLength(128)]
        public string? Description { get; set; }

        public FeatureWeight Weight { get; set; }

        public int MaxPoints { get; set; }


        public Competition Competition { get; private set; } = null!;
        public SingingFeature SingingFeature { get; private set; } = null!;

        public List<Score> Scores { get; } = new List<Score>();


        public EvaluatedFeature()
        {
        }

        public EvaluatedFeature(Competition competition, SingingFeature feature, int? points = null)
        {
            CompetitionID = competition.ID;
            SingingFeatureID = feature.ID;
            Name = feature.Name;
            Description = feature.Description;
            Weight = feature.Weight;
            MaxPoints = points ?? feature.DefaultMaxPoints;
        }


        public bool IsPositive() => (Weight == FeatureWeight.Positive);

        public bool IsNegative() => (Weight == FeatureWeight.Negative);
    }
}

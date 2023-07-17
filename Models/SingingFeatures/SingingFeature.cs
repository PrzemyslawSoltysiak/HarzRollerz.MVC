using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HarzRollerz.MVC.Models.SingingFeatures
{
    public class SingingFeature
    {
        [Key]
        public int ID { get; set; }

        [Required, StringLength(32)]
        public string Name { get; set; } = string.Empty;

        [StringLength(128)]
        public string? Description { get; set; }

        public FeatureWeight Weight { get; set; }

        [Display(Name = "Default Max Points")]
        public int DefaultMaxPoints { get; set; }


        public List<EvaluatedFeature> Evaluations { get; set; } = new List<EvaluatedFeature>();


        public SingingFeature()
        {
        }

        public SingingFeature(string name, FeatureWeight weight, int points, string? description = null)
        {
            Name = name;
            Weight = weight;
            DefaultMaxPoints = points;
            Description = description;
        }


        public bool IsPositive() => (Weight == FeatureWeight.Positive);

        public bool IsNegative() => (Weight == FeatureWeight.Negative);
    }
}

using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.SingingFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace HarzRollerz.MVC.ViewModels.Competitions
{
    public class FeatureToEvaluate
    {
        public int SingingFeatureID { get; set; }
        public string SingingFeatureName { get; set; } = string.Empty;
        public string SingingFeatureWeight { get; set; } = string.Empty;
        public bool IsSelected { get; set; }
        public int MaxPoints { get; set; }

        public FeatureToEvaluate() { }

        public FeatureToEvaluate(SingingFeature feature)
        {
            SingingFeatureID = feature.ID;
            SingingFeatureName = feature.Name;
            SingingFeatureWeight = feature.Weight.ToString();
            IsSelected = true;
            MaxPoints = feature.DefaultMaxPoints;
        }
    }

    public class CreateCompetitionViewModel
    {
        [Required, StringLength(128)]
        public string Name { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [StringLength(32)]
        public string? Place { get; set; }

        [StringLength(32)]
        public string? Rank { get; set; }

        [Range(1, 4)]
        [Display(Name = "Collection Size")]
        public int CollectionSize { get; set; } = 4;

        public FeatureToEvaluate[] FeaturesToEvaluate { get; set; } = new FeatureToEvaluate[0];

        public CreateCompetitionViewModel() { }

        public CreateCompetitionViewModel(Competition competition, FeatureToEvaluate[] features)
        {
            Name = competition.Name;
            Date = competition.Date;
            Place = competition.Place;
            Rank = competition.Rank;
            CollectionSize = competition.CollectionSize;
            FeaturesToEvaluate = features;
        }
    }
}

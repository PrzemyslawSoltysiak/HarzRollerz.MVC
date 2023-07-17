using HarzRollerz.MVC.Models.SingingFeatures;
using HarzRollerz.MVC.Services;

namespace HarzRollerz.MVC.ViewModels.SingingFeatures
{
	public class SingingFeatureViewModel
	{
		public int ID { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
		public FeatureWeight Weight { get; set; }
		public int DefaultMaxPoints { get; set; }
		public int? BestPercentageScore { get; set; }

		public SingingFeatureViewModel()
		{
		}

		public SingingFeatureViewModel(SingingFeature feature, int? best_percentage_score)
		{
			ID = feature.ID;
			Name = feature.Name;
			Description = feature.Description;
			Weight = feature.Weight;
			DefaultMaxPoints = feature.DefaultMaxPoints;
			BestPercentageScore = best_percentage_score;
		}

		public bool IsPositive() => (Weight == FeatureWeight.Positive);
		public bool IsNegative() => (Weight == FeatureWeight.Negative);
	}

	public class SingingFeaturesViewModel
	{
		public List<SingingFeatureViewModel> SingingFeatures { get; set; } = new List<SingingFeatureViewModel>();

		public SingingFeaturesViewModel()
		{
		}

		public SingingFeaturesViewModel(IEnumerable<SingingFeature> features, ScoresService scores_service)
		{
			foreach (var feature in features)
			{
				//SingingFeatures.Add(new SingingFeatureViewModel()
				//{
				//	ID = feature.ID,
				//	Name = feature.Name,
				//	Description = feature.Description,
				//	Weight = feature.Weight,
				//	DefaultMaxPoints = feature.DefaultMaxPoints,
				//	BestPercentageScore = scores_service.GetBestPercentageScoreForSingingFeature(feature.ID)
				//});
				int? best_percentage_score = scores_service.GetBestPercentageScoreForEvaluatedFeature(feature.ID);
				SingingFeatures.Add(new SingingFeatureViewModel(feature, best_percentage_score));
			}
		}
	}
}

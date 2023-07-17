using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Scores;
using HarzRollerz.MVC.Models.SingingFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HarzRollerz.MVC.ViewModels.EvaluationCard
{
    public class ScoreViewModel
    {
        public int? ID { get; set; }
        public int CageID { get; set; }
        public int EvaluatedFeatureID { get; set; }
        public int? Points { get; set; }

        public ScoreViewModel()
        {
        }

        public ScoreViewModel(Cage cage, EvaluatedFeature feature, int? points = null)
        {
            CageID = cage.ID;
            EvaluatedFeatureID = feature.ID;
            Points = points;
        }

        public ScoreViewModel(Score score)
        {
            ID = score.ID;
            CageID = score.CageID;
            EvaluatedFeatureID = score.EvaluatedFeatureID;
            Points = score.Points;
        }
    }

    public class EvaluationCardViewModel
    {
        public int CompetitionID { get; set; }
        public int CollectionID { get; set; }

        [ValidateNever]
        public Competition Competition { get; set; } = null!;
        [ValidateNever]
        public Collection Collection { get; set; } = null!;

        public ScoreViewModel[] Scores { get; set; } = new ScoreViewModel[0];

        public EvaluationCardViewModel()
        {
        }

        public EvaluationCardViewModel(Competition competition, Collection collection)
        {
            CompetitionID = competition.ID;
            Competition = competition;
            CollectionID = collection.ID;
            Collection = collection;
            var scores = new List<ScoreViewModel>();
            foreach (var feature in competition.EvaluatedFeatures)
            {
                foreach (var cage in collection.Cages)
                {
                    scores.Add(new ScoreViewModel(cage, feature));
                }
            }
            Scores = scores.ToArray();
        }

        public EvaluationCardViewModel(Competition competition, Collection collection, Score[] scores)
        {
            CompetitionID = competition.ID;
            Competition = competition;
            CollectionID = collection.ID;
            Collection = collection;
            Scores = scores.Select(s => new ScoreViewModel(s)).ToArray();
        }

        public EvaluationCardViewModel(Competition competition, Collection collection, ScoreViewModel[] scores)
        {
            CompetitionID = competition.ID;
            Competition = competition;
            CollectionID = collection.ID;
            Collection = collection;
            var scores_list = new List<ScoreViewModel>();
            foreach (var score in scores)
            {
                var cage = collection.Cages.First(c => c.ID == score.CageID);
                var feature = competition.EvaluatedFeatures.First(ef => ef.ID == score.EvaluatedFeatureID);
                scores_list.Add(new ScoreViewModel(cage, feature, score.Points));
            }
            Scores = scores.ToArray();
        }
    }
}

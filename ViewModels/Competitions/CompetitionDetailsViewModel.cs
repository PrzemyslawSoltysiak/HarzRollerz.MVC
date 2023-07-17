using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Models.SingingFeatures;
using HarzRollerz.MVC.Services;

namespace HarzRollerz.MVC.ViewModels.Competitions
{
    public class EvaluatedFeatureViewModel
    {
        public int ID { get; set; }
        public int SingingFeatureID { get; set; }
        public string Name { get; set; } = string.Empty;
        public FeatureWeight Weight { get; set; }
        public int MaxPoints { get; set; }
        public bool HasScores { get; set; }
        public int? BestScore { get; set; }
        public int? BestPercentageScore { get; set; }
        
        public EvaluatedFeatureViewModel() { }

        public EvaluatedFeatureViewModel(EvaluatedFeature feature, ScoresService scores_service)
        {
            ID = feature.ID;
            SingingFeatureID = feature.SingingFeatureID;
            Name = feature.Name;
            Weight = feature.Weight;
            MaxPoints = feature.MaxPoints;
            HasScores = scores_service.EvaluatedFeatureHasScores(feature.ID);
            BestScore = scores_service.GetBestPercentageScoreForEvaluatedFeature(feature.ID);
            BestPercentageScore = scores_service.GetBestPercentageScoreForEvaluatedFeature(feature.ID);
        }
    }

    public class CageViewModel
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public string RingNumber { get; set; } = string.Empty;
        public bool HasScores { get; set; }
        public int? TotalScore { get; set; }
        public int? TotalPercentageScore { get; set; }

        public CageViewModel() { }
        
        public CageViewModel(Cage cage, ScoresService scores_service)
        {
            ID = cage.ID;
            Number = cage.Number;
            RingNumber = cage.Canary.RingNumber;
            HasScores = scores_service.CageHasScores(cage.ID);
            TotalScore = scores_service.GetCageTotalScore(cage.ID);
            TotalPercentageScore = scores_service.GetCagePercentageScore(cage.ID);
        }
    }

    public class CollectionViewModel
    {
        public int ID { get; set; }
        public int? Rank { get; set; }
        public string OwnerFirstLastName { get; set; } = string.Empty;
        public string OwnerSignature { get; set; } = string.Empty;
        public bool HasScores { get; set; }
        public int? TotalScore { get; set; }
        public int? TotalPercentageScore { get; set; }
        public List<CageViewModel> Cages { get; } = new List<CageViewModel>();

        public CollectionViewModel() { }

        public CollectionViewModel(Collection collection, ScoresService scores_service)
        {
            ID = collection.ID;
            Rank = scores_service.GetCollectionRankingInCompetition(collection.ID);
            OwnerFirstLastName = collection.Owner.FirstLastName;
            OwnerSignature = collection.Owner.Signature;
            HasScores = scores_service.CollectionHasScores(collection.ID);
            TotalScore = scores_service.GetCollectionTotalScore(collection.ID);
            TotalPercentageScore = scores_service.GetCollectionPercentageScore(collection.ID);
            foreach (var cage in collection.Cages)
            {
                Cages.Add(new CageViewModel(cage, scores_service));
            }
        }
    }

    public class CompetitionDetailsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Place { get; set; }
        public DateTime? Date { get; set; }
        public string? Rank { get; set; }
        public int CollectionSize { get; set; }
        public List<EvaluatedFeatureViewModel> EvaluatedFeatures { get; } = new List<EvaluatedFeatureViewModel>();
        public List<CollectionViewModel> RegisteredCollections { get; } = new List<CollectionViewModel>();

        public CompetitionDetailsViewModel() { }

        public CompetitionDetailsViewModel(Competition competition, ScoresService scores_service)
        {
            ID = competition.ID;
            Name = competition.Name;
            Place = competition.Place;
            Date = competition.Date;
            Rank = competition.Rank;
            CollectionSize = competition.CollectionSize;
            foreach (var feature in competition.EvaluatedFeatures.OrderByDescending(ef => ef.Weight).ThenBy(ef => ef.ID))
            {
                EvaluatedFeatures.Add(new EvaluatedFeatureViewModel(feature, scores_service));
            }
            foreach (var collection in competition.Collections)
            {
                RegisteredCollections.Add(new CollectionViewModel(collection, scores_service));
            }
        }
    }
}

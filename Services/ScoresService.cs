using HarzRollerz.MVC.Data;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Services
{
	public class ScoresService
	{
		private readonly DataContext db;

		public ScoresService(DataContext context)
		{
			db = context;
		}


		public bool IsScoreValid(int efID, int points)
		{
			var feature = db.EvaluatedFeatures.First(ef => ef.ID == efID);
			return (points <= feature.MaxPoints);
		}

		
		public bool EvaluatedFeatureHasScores(int efID)
		{
			return db.Scores.Any(s => (s.EvaluatedFeatureID == efID) && (s.Points != null));
		}


		public int GetCageMaxPositivePoints(int competitionID)
		{
			return db.EvaluatedFeatures
				.ToList()
				.Where(ef => ef.IsPositive() && ef.CompetitionID == competitionID)
				.Sum(ef => ef.MaxPoints);
		}

		public int GetCageMaxNegativePoints(int competitionID)
		{
			return db.EvaluatedFeatures
				.ToList()
				.Where(ef => ef.IsNegative() && ef.CompetitionID == competitionID)
				.Sum(ef => ef.MaxPoints);
		}

		public int GetCageMaxPoints(int competitionID)
		{
			int positive = GetCageMaxPositivePoints(competitionID);
			int negative = GetCageMaxNegativePoints(competitionID);
			return positive - negative;
		}


		public int GetCollectionMaxPositivePoints(int competitionID)
		{
			var competition = db.Competitions.First(c => c.ID == competitionID);
			return GetCageMaxPositivePoints(competitionID) * competition.CollectionSize;
		}

		public int GetCollectionMaxNegativePoints(int competitionID)
		{
			var competition = db.Competitions.First(c => c.ID == competitionID);
			return GetCageMaxNegativePoints(competitionID) * competition.CollectionSize;
		}

		public int GetCollectionMaxPoints(int competitionID)
		{
			int positive = GetCollectionMaxPositivePoints(competitionID);
			int negative = GetCollectionMaxNegativePoints(competitionID);
			return positive - negative;
		}


		public bool CageHasScores(int cageID)
		{
			return db.Scores.Where(s => (s.CageID == cageID) && (s.Points != null)).Any();
		}

		public int GetCageTotalPositiveScore(int cageID)
		{
			return db.Scores
				.Where(s => s.CageID == cageID)
				.Include(s => s.EvaluatedFeature)
				.ToList()
				.Where(s => s.EvaluatedFeature.IsPositive())
				.Sum(s => s.Points ?? 0);
		}

		public int GetCageTotalNegativeScore(int cageID)
		{
			return db.Scores
				.Where(s => s.CageID == cageID)
				.Include(s => s.EvaluatedFeature)
				.ToList()
				.Where(s => s.EvaluatedFeature.IsNegative())
				.Sum(s => s.Points ?? 0);
		}

		public int GetCageTotalScore(int cageID)
		{
			int positive = GetCageTotalPositiveScore(cageID);
			int negative = GetCageTotalNegativeScore(cageID);
			return positive - negative;
		}

		public int? GetCagePercentageScore(int cageID)
		{
			if (!db.Scores.Any(s => s.CageID == cageID))
			{
				return null;
			}
			var cage = db.Cages.Include(c => c.Collection).First(c => c.ID == cageID);
			double value = (double)GetCageTotalScore(cageID) / GetCageMaxPoints(cage.Collection.CompetitionID);
			return (int)Math.Round(value * 100);
		}

		public int? GetCageRankingInCompetition(int cageID)
		{
			var cage = db.Cages.First(c => c.ID == cageID);
			var ranking = db.Cages.ToList().OrderByDescending(c => GetCageTotalScore(c.ID));
			return ranking.ToList().IndexOf(cage) + 1;
		}


		public bool CollectionHasScores(int collectionID)
		{
			return db.Cages
				.Where(c => c.CollectionID == collectionID)
				.ToList()
				.Any(c => CageHasScores(c.ID));
		}

		public int GetCollectionTotalPositiveScore(int collectionID)
		{
			return db.Cages
				.Where(c => c.CollectionID == collectionID)
				.ToList()
				.Sum(c => GetCageTotalPositiveScore(c.ID));
		}

		public int GetCollectionTotalNegativeScore(int collectionID)
		{
			return db.Cages
				.Where(c => c.CollectionID == collectionID)
				.ToList()
				.Sum(c => GetCageTotalNegativeScore(c.ID));
		}

		public int GetCollectionTotalScore(int collectionID)
		{
			int positive = GetCollectionTotalPositiveScore(collectionID);
			int negative = GetCollectionTotalNegativeScore(collectionID);
			return positive - negative;
		}

		public int? GetCollectionPercentageScore(int collectionID)
		{
			if (!db.Scores.Any(s => s.Cage.CollectionID == collectionID))
			{
				return null;
			}
			var collection = db.Collections.First(c => c.ID == collectionID);
			double value = (double)GetCollectionTotalScore(collectionID) / GetCollectionMaxPoints(collection.CompetitionID);
			return (int)Math.Round(value * 100);
		}

		public int GetCollectionRankingInCompetition(int collectionID)
		{
			var collection = db.Collections.First(c => c.ID == collectionID);
			var ranking = db.Collections.Where(c => c.CompetitionID == collection.CompetitionID).ToList().OrderByDescending(c => GetCollectionTotalScore(c.ID));
			return ranking.ToList().IndexOf(collection) + 1;
		}


		public int? GetBestScoreForEvaluatedFeature(int efID)
		{
			if (!db.Scores.Any(s => (s.EvaluatedFeatureID == efID) && (s.Points != null)))
			{
				return null;
			}

			var ef = db.EvaluatedFeatures.First(ef => ef.ID == efID);
			if (ef.IsPositive())
			{
				return db.Scores.Where(s => s.EvaluatedFeatureID == efID).Max(s => s.Points ?? 0);
			}
			else
			{
				return db.Scores.Where(s => s.EvaluatedFeatureID == efID).Max(s => (-1) * s.Points ?? 0);
			}
		}

		public int? GetBestPercentageScoreForEvaluatedFeature(int efID)
		{
			if (!db.Scores.Any(s => (s.EvaluatedFeatureID == efID) && (s.Points != null)))
			{
				return null;
			}

			var ef = db.EvaluatedFeatures.First(ef => ef.ID == efID);
			if (ef.IsPositive())
			{
				return (int?)db.Scores.Where(s => s.EvaluatedFeatureID == efID).Max(s => (double)(s.Points ?? 0) / s.EvaluatedFeature.MaxPoints * 100);
			}
			else
			{
				return (int?)db.Scores.Where(s => s.EvaluatedFeatureID == efID).Min(s => (double)(s.Points ?? 0) / s.EvaluatedFeature.MaxPoints * 100);
			}
		}

		public int? GetBestPercentageScoreForSingingFeature(int sfID)
		{
			if (!db.Scores.Any(s => s.EvaluatedFeature.SingingFeatureID == sfID))
			{
				return null;
			}

			var feature = db.SingingFeatures.First(sf => sf.ID == sfID);
			if (feature.IsPositive())
			{
				return db.EvaluatedFeatures.Where(ef => ef.SingingFeatureID == sfID).ToList().Max(ef => GetBestPercentageScoreForEvaluatedFeature(ef.ID));
			}
			else
			{
				return db.EvaluatedFeatures.Where(ef => ef.SingingFeatureID == sfID).ToList().Min(ef => GetBestPercentageScoreForEvaluatedFeature(ef.ID));
			}
		}


		public int? GetBestBreedersCollectionRank(int ownerID)
		{
			if (!db.Scores.Any(s => s.Cage.Collection.OwnerID == ownerID))
			{
				return null;
			}
			var collections = db.Collections.Where(c => c.OwnerID == ownerID).ToList();
			return collections.Min(c => GetCollectionRankingInCompetition(c.ID));
		}

		public int? GetBestBreedersCanaryRank(int ownerID)
		{
			if (!db.Scores.Any(s => s.Cage.Canary.OwnerID == ownerID))
			{
				return null;
			}
			var cages = db.Cages.Where(c => c.Canary.OwnerID == ownerID).ToList();
			return cages.Min(c => GetCageRankingInCompetition(c.ID));
		}
	}
}

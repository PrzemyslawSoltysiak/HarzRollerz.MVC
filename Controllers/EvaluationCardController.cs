using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.Scores;
using HarzRollerz.MVC.ViewModels.EvaluationCard;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
	public class EvaluationCardController : Controller
    {
        private readonly DataContext _context;

        public EvaluationCardController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int competitionID, int? collectionID)
        {
            var competitions = _context.Competitions
                .Include(c => c.EvaluatedFeatures)
                    .ThenInclude(ef => ef.SingingFeature)
                .Include(c => c.Collections)
                    .ThenInclude(c => c.Cages)
                        .ThenInclude(c => c.Canary)
                .Include(c => c.Collections)
                    .ThenInclude(c => c.Owner);

            var competition = competitions.FirstOrDefault(c => c.ID == competitionID);
            if (competition == null)
            {
                return NotFound();
            }

			var collection = competition.Collections.FirstOrDefault(c => c.ID == collectionID);
			if (collection == null)
			{
				return NotFound();
			}

            var scores = _context.Scores
                .Include(s => s.Cage)
                .Where(s => s.Cage.CollectionID == collectionID);

            if (scores.Any())
            {
				var model = new EvaluationCardViewModel(competition, collection, scores.ToArray());
				return View(model);
			}
            else
            {
				var model = new EvaluationCardViewModel(competition, collection);
				return View(model);
			}
        }

        [HttpPost]
        public IActionResult Index(EvaluationCardViewModel model)
        {
			var competitions = _context.Competitions
				.Include(c => c.EvaluatedFeatures)
					.ThenInclude(ef => ef.SingingFeature)
				.Include(c => c.Collections)
					.ThenInclude(c => c.Cages)
						.ThenInclude(c => c.Canary)
				.Include(c => c.Collections)
					.ThenInclude(c => c.Owner);

			var competition = competitions.FirstOrDefault(c => c.ID == model.CompetitionID);
            if (competition == null)
            {
                return NotFound();
            }
            model.Competition = competition;

			var collection = competition.Collections.FirstOrDefault(c => c.ID == model.CollectionID);
            if (collection == null)
            {
                return NotFound();
            }
            model.Collection = collection;

            var scores = _context.Scores.Where(s => s.Cage.CollectionID == model.CollectionID).ToList();

            if (scores.Any())
            {
                model = new EvaluationCardViewModel(competition, collection, scores.ToArray());
                return View(model);
            }
            
			foreach (var score_vm in model.Scores)
            {
                var feature = _context.EvaluatedFeatures
                    .Where(ef => ef.ID == score_vm.EvaluatedFeatureID)
                    .FirstOrDefault();

                var cage = _context.Cages
                    .Where(c => c.ID == score_vm.CageID)
                    .FirstOrDefault();

                if ((feature == null) || (cage == null))
                {
                    return NotFound();
                }

                if (score_vm.Points > feature.MaxPoints)
                {
                    ModelState.AddModelError(nameof(score_vm), "Max. " + feature.MaxPoints + " points.");
                }
                else
                {
                    scores.Add(new Score(cage, feature, score_vm.Points));
                }
            }

            if (!TryValidateModel(model.Scores))
            {
                model = new EvaluationCardViewModel(competition, collection, model.Scores);
                return View(model);
            }

            _context.AddRange(scores);
            _context.SaveChanges();

            model = new EvaluationCardViewModel(competition, collection, scores.ToArray());
            return View(model);
        }
    }
}

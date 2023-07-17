using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.Competitions;
using HarzRollerz.MVC.Models.SingingFeatures;
using HarzRollerz.MVC.Services;
using HarzRollerz.MVC.ViewModels.Competitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
	public class CompetitionsController : Controller
    {
        private readonly DataContext _context;

        public CompetitionsController(DataContext context)
        {
            _context = context;
        }

        // GET: Competitions
        public async Task<IActionResult> Index()
        {
            return _context.Competitions != null ?
                        View(await _context.Competitions.ToListAsync()) :
                        Problem("Entity set 'DataContext.Competitions'  is null.");
        }

        // GET: Competitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Competitions == null)
            {
                return NotFound();
            }

            var competition = await _context.Competitions
                .Include(c => c.EvaluatedFeatures)
                    .ThenInclude(ef => ef.SingingFeature)
                .Include(c => c.Collections)
                    .ThenInclude(c => c.Cages)
                        .ThenInclude(c => c.Canary)
                .Include(c => c.Collections)
                    .ThenInclude(c => c.Cages)
                        .ThenInclude(c => c.Scores)
                .Include(c => c.Collections)
                    .ThenInclude(c => c.Owner)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (competition == null)
            {
                return NotFound();
            }

            var model = new CompetitionDetailsViewModel(competition, new ScoresService(_context));
            return View(model);
        }

        // GET: Competitions/Create
        public IActionResult Create()
        {
            ViewData["SingingFeatures"] = _context.SingingFeatures.ToList();
            var competition = new Competition();
            var features = _context.SingingFeatures.Select(sf => new FeatureToEvaluate(sf));
            var model = new CreateCompetitionViewModel(competition, features.ToArray());
            return View(model);
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCompetitionViewModel model)
        {
            var competition = new Competition(model.Name, model.Date, model.Place, model.Rank, model.CollectionSize);

            foreach (var feature_to_evaluate in model.FeaturesToEvaluate.Where(f => f.IsSelected))
            {
                var singing_feature = _context.SingingFeatures.FirstOrDefault(sf => sf.ID == feature_to_evaluate.SingingFeatureID);
                if (singing_feature == null)
                {
                    return NotFound();
                }

                var evaluated_feature = new EvaluatedFeature(competition, singing_feature, feature_to_evaluate.MaxPoints);
                competition.EvaluatedFeatures.Add(evaluated_feature);
            }

            if (!TryValidateModel(model, nameof(model)))
            {
                return View(model);
            }

            _context.Competitions.Add(competition);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}

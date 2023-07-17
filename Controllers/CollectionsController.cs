using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.Collections;
using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Services;
using HarzRollerz.MVC.ViewModels.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
	public class CollectionsController : Controller
    {
        private readonly DataContext _context;
        private readonly ScoresService scores_service;

        public CollectionsController(DataContext context)
        {
            _context = context;
            scores_service = new ScoresService(context);
        }

        // GET: Collections/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Collections == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .Include(c => c.Competition)
                .Include(c => c.Owner)
                .Include(c => c.Cages)
                    .ThenInclude(c => c.Canary)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (collection == null)
            {
                return NotFound();
            }

            var model = CreateCollectionDetailsViewModel(id.Value);

            return View(model);
        }

        // GET: Collections/Create
        public IActionResult Create(int competitionID, int? ownerID)
        {
            var competition = _context.Competitions.Find(competitionID);
            if (competition == null)
            {
                return NotFound();
            }

            var available_owners = _context.Breeders.Where(b => !b.Collections.Any(c => c.CompetitionID == competitionID));

            if (ownerID == null || !available_owners.Select(b => b.ID).ToList().Contains((int)ownerID))
            {
                var model = new CreateCollectionViewModel(competition, available_owners);
                return View(model);
            }
            else
            {
                var selected_owner = available_owners.FirstOrDefault(b => b.ID == ownerID);
                if (selected_owner == null)
                {
                    return NotFound();
                }
                var owners_canaries = _context.Canaries.Where(c => c.OwnerID == ownerID);
                var model = new CreateCollectionViewModel(competition, available_owners, selected_owner, owners_canaries);
                return View(model);
            }
        }

        // POST: Collections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCollectionViewModel model)
        {
            var competition = _context.Competitions.Find(model.CompetitionID);
            if (competition == null)
            {
                return NotFound();
            }

            var available_owners = _context.Breeders.Where(b => !b.Collections.Any(c => c.CompetitionID == model.CompetitionID));

            if (model.OwnerID == null || !available_owners.Select(b => b.ID).ToList().Contains((int)model.OwnerID))
            {
                model = new CreateCollectionViewModel(competition, available_owners);
                return View(model);
            }

            var selected_owner = available_owners.FirstOrDefault(b => b.ID == model.OwnerID);
            if (selected_owner == null)
            {
                return NotFound();
            }
            var owners_canaries = _context.Canaries.Where(c => c.OwnerID == model.OwnerID).ToList();

            Canary?[] selected_canaries = new Canary?[competition.CollectionSize];
            for (int cage_number = 1; cage_number <= competition.CollectionSize; ++cage_number)
            {
                string? ring_number = model.Cages[cage_number - 1].CanaryRingNumber;
                if (ring_number != null)
                {
                    var canary = owners_canaries.FirstOrDefault(c => c.RingNumber == ring_number);
                    if (canary == null)
                    {
                        return NotFound();
                    }
                    selected_canaries[cage_number - 1] = canary;
                }
            }

            if (selected_canaries.Where(c => c != null).Distinct().Count() != competition.CollectionSize)
            {
                model = new CreateCollectionViewModel(competition, available_owners, selected_owner, owners_canaries, selected_canaries);
                return View(model);
            }

            var collection = new Collection(competition, selected_owner);
            for (int cage_number = 1; cage_number <= competition.CollectionSize; ++cage_number)
            {
                var cage = new Cage(collection, selected_canaries[cage_number - 1]!, cage_number);
                collection.Cages.Add(cage);
            }

            _context.Add(collection);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Collections", new { collection.ID });
        }


        private CollectionDetailsViewModel CreateCollectionDetailsViewModel(int id)
        {
            var collection = _context.Collections
                .Include(c => c.Cages)
                .ThenInclude(c => c.Canary)
                .First(c => c.ID == id);

            var model = new CollectionDetailsViewModel()
            {
                ID = id,
                Competition = collection.Competition,
                Owner = collection.Owner,
                IsEvaluated = scores_service.CollectionHasScores(id)
            };

            foreach (var cage in collection.Cages.OrderBy(c => c.Number))
            {
                var cage_model = new CageDetailsViewModel()
                {
                    ID = cage.ID,
                    CanaryID = cage.CanaryID,
                    Number = cage.Number,
                    RingNumber = cage.Canary!.RingNumber,
                    IsEvaluated = scores_service.CageHasScores(cage.ID)
                };
                if (cage_model.IsEvaluated)
                {
                    cage_model.TotalPositivePoints = scores_service.GetCageTotalPositiveScore(cage.ID);
                    cage_model.TotalNegativePoints = scores_service.GetCageTotalNegativeScore(cage.ID);
                    cage_model.TotalPoints = scores_service.GetCageTotalScore(cage.ID);
                    cage_model.TotalScore = scores_service.GetCagePercentageScore(cage.ID);
                    cage_model.Rank = scores_service.GetCageRankingInCompetition(cage.ID);
                }
                model.Cages.Add(cage_model);
            }

            if (model.IsEvaluated)
            {
                model.TotalPositivePoints = scores_service.GetCollectionTotalPositiveScore(collection.ID);
                model.TotalNegativePoints = scores_service.GetCollectionTotalNegativeScore(collection.ID);
                model.TotalPoints = scores_service.GetCollectionTotalScore(collection.ID);
                model.TotalScore = scores_service.GetCollectionPercentageScore(collection.ID);
                model.Rank = scores_service.GetCollectionRankingInCompetition(collection.ID);
            }

            return model;
        }
    }
}

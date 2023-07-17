using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.SingingFeatures;
using HarzRollerz.MVC.Services;
using HarzRollerz.MVC.ViewModels.SingingFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
    public class SingingFeaturesController : Controller
    {
        private readonly DataContext _context;
        private readonly ScoresService scores_service;

        public SingingFeaturesController(DataContext context)
        {
            _context = context;
			scores_service = new ScoresService(context);

		}

        // GET: SingingFeatures
        public async Task<IActionResult> Index()
        {
            if (_context.SingingFeatures == null)
            {
                return Problem("Entity set 'DataContext.SingingFeatures' is null.");
            }

            var features = await _context.SingingFeatures.ToListAsync();
            var model = new SingingFeaturesViewModel(features, scores_service);
            return View(model);

        }

        // GET: SingingFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SingingFeatures == null)
            {
                return NotFound();
            }

            var singingFeature = await _context.SingingFeatures
                .FirstOrDefaultAsync(m => m.ID == id);
            if (singingFeature == null)
            {
                return NotFound();
            }

            return View(singingFeature);
        }

        // GET: SingingFeatures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SingingFeatures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Description")] SingingFeature singingFeature)
        {
            if (ModelState.IsValid)
            {
                _context.Add(singingFeature);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(singingFeature);
        }

        // GET: SingingFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SingingFeatures == null)
            {
                return NotFound();
            }

            var singingFeature = await _context.SingingFeatures.FindAsync(id);
            if (singingFeature == null)
            {
                return NotFound();
            }
            return View(singingFeature);
        }

        // POST: SingingFeatures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,Weight,DefaultMaxPoints")] SingingFeature singingFeature)
        {
            if (id != singingFeature.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(singingFeature);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SingingFeatureExists(singingFeature.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(singingFeature);
        }

        private bool SingingFeatureExists(int id)
        {
          return (_context.SingingFeatures?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}

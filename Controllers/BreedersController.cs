using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.Entities;
using HarzRollerz.MVC.Services;
using HarzRollerz.MVC.ViewModels.Breeders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
	public class BreedersController : Controller
    {
        private readonly DataContext _context;

        public BreedersController(DataContext context)
        {
            _context = context;
        }

        // GET: Breeders
        public async Task<IActionResult> Index()
        {
            if (_context.Breeders == null)
            {
                return Problem("Entity set 'DataContext.Breeders' is null.");
            }

            var breeders = await _context.Breeders.ToListAsync();
            return View(breeders);
        }

        // GET: Breeders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Breeders == null)
            {
                return NotFound();
            }

            var breeder = await _context.Breeders
                .Include(b => b.Canaries)
                .Include(b => b.Collections)
                    .ThenInclude(c => c.Cages)
                        .ThenInclude(c => c.Scores)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (breeder == null)
            {
                return NotFound();
            }

            var model = new BreederDetailsViewModel(breeder, breeder.Canaries, new ScoresService(_context));
            return View(model);
        }

        // GET: Breeders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Breeders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName")] Breeder breeder)
        {
            breeder.Signature = GenerateSignature(breeder);
            breeder.RegistrationDate = DateTime.UtcNow;

            ModelState.ClearValidationState(nameof(breeder));
            if (!TryValidateModel(breeder, nameof(breeder)))
            {
                return View(breeder);
            }

            _context.Breeders.Add(breeder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string GenerateSignature(Breeder breeder)
        {
            char letter = char.ToUpper(breeder.LastName[0]);
            var group = _context.Breeders.ToList().Where(b => b.Signature.StartsWith(letter));
            var last = group.OrderByDescending(b => b.Signature).FirstOrDefault();
            int index = (last != null) ? Int32.Parse(last.Signature.Skip(1).ToArray()) + 1 : 1;
            return letter + index.ToString("D3");
        }
    }
}

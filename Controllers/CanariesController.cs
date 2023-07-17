using HarzRollerz.MVC.Data;
using HarzRollerz.MVC.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HarzRollerz.MVC.Controllers
{
	public class CanariesController : Controller
    {
        private readonly DataContext _context;

        public CanariesController(DataContext context)
        {
            _context = context;
        }

        // GET: Canaries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Canaries == null)
            {
                return NotFound();
            }

            var canary = await _context.Canaries
                .Include(c => c.Owner)
                .Include(c => c.Cages)
                    .ThenInclude(c => c.Collection)
                        .ThenInclude(c => c.Competition)
                .FirstOrDefaultAsync(m => m.ID == id);

            if (canary == null)
            {
                return NotFound();
            }

            return View(canary);
        }

        // GET: Canaries/Create
        public IActionResult Create(int ownerID)
        {
            var owner = _context.Breeders.FirstOrDefault(b => b.ID == ownerID);
            if (owner == null)
            {
                return NotFound();
            }

            var owners_canaries = _context.Canaries.Where(c => c.OwnerID == ownerID);
            if (!owners_canaries.Any())
            {
                return View(new Canary(owner, 1));
            }

            int no = owners_canaries.Max(c => c.OrdinalNumber) + 1;
            return View(new Canary(owner, no));
        }

        // POST: Canaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,OwnerID,InnerRingDiameter,OrdinalNumber,Name")] Canary canary)
        {
            var owner = _context.Breeders.FirstOrDefault(b => b.ID == canary.OwnerID);
            if (owner == null)
            {
                return NotFound();
            }
            // canary.Owner = owner;

            ModelState.Remove("Owner");
            if (!ModelState.IsValid)
            {
                return View(canary);
            }

            _context.Canaries.Add(canary);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Breeders", new { id = canary.OwnerID });
        }
    }
}

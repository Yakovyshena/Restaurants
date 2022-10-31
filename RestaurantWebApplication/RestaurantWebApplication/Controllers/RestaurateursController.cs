#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RestaurantWebApplication.Controllers
{
    public class RestaurateursController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public RestaurateursController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Restaurateurs
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsContext = _context.Restaurateurs.Include(r => r.BirthCity);
            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Restaurateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurateur = await _context.Restaurateurs
                .Include(r => r.BirthCity)
                .FirstOrDefaultAsync(m => m.RestaurateurId == id);
            if (restaurateur == null)
            {
                return NotFound();
            }

            return View(restaurateur);
        }

        // GET: Restaurateurs/Create
        public IActionResult Create()
        {
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            return View();
        }

        // POST: Restaurateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurateurId,FirstName,MiddleName,LastName,BirthCityId")] Restaurateur restaurateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurateur.BirthCityId);
            return View(restaurateur);
        }

        // GET: Restaurateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurateur = await _context.Restaurateurs.FindAsync(id);
            if (restaurateur == null)
            {
                return NotFound();
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurateur.BirthCityId);
            return View(restaurateur);
        }

        // POST: Restaurateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurateurId,FirstName,MiddleName,LastName,BirthCityId")] Restaurateur restaurateur)
        {
            if (id != restaurateur.RestaurateurId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurateurExists(restaurateur.RestaurateurId))
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
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurateur.BirthCityId);
            return View(restaurateur);
        }

        // GET: Restaurateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurateur = await _context.Restaurateurs
                .Include(r => r.BirthCity)
                .FirstOrDefaultAsync(m => m.RestaurateurId == id);
            if (restaurateur == null)
            {
                return NotFound();
            }

            return View(restaurateur);
        }

        // POST: Restaurateurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurateur = await _context.Restaurateurs.FindAsync(id);
            _context.Restaurateurs.Remove(restaurateur);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurateurExists(int id)
        {
            return _context.Restaurateurs.Any(e => e.RestaurateurId == id);
        }
    }
}

#nullable disable
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RestaurantWebApplication.Controllers
{
    public class ChefsController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public ChefsController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Chefs
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsContext = _context.Chefs.Include(c => c.BirthCity);

            List<Tuple<Chef, List<Restaurant>, List<Menu>>> chefsWithInfo = new List<Tuple<Chef, List<Restaurant>, List<Menu>>>();

            foreach(Chef c in _context.Chefs)
            {
                chefsWithInfo.Add(new Tuple<Chef, List<Restaurant>, List<Menu>>(
                    c,
                    FindRestaurants(c.ChefId),
                    FindMenus(c.ChefId)));
            }

            ViewBag.ChefsWithInfo = chefsWithInfo;

            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Chefs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .Include(c => c.BirthCity)
                .FirstOrDefaultAsync(m => m.ChefId == id);
            if (chef == null)
            {
                return NotFound();
            }

            ViewBag.Restaurants = FindRestaurants(chef.ChefId);
            ViewBag.Specialisations = FindSpecialisations(chef.ChefId);

            return View(chef);
        }

        public async Task<IActionResult> Specializations(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .FirstOrDefaultAsync(m => m.ChefId == id);
            if (chef == null)
            {
                return NotFound();
            }

            ViewBag.Specializations = _context.Specialisations.ToList();
            ViewBag.ChefSpecializations = FindSpecialisations(chef.ChefId);

            return View(chef);
        }

        // GET: Chefs/Create
        public IActionResult Create()
        {
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId");

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSpecialization([Bind("PairId,ChefId,SpecialisationId")] ChefsCpecialisation chefsCpecialisation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chefsCpecialisation);
                await _context.SaveChangesAsync();
            }

            if (chefsCpecialisation == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs.FindAsync(chefsCpecialisation.ChefId);
            if (chef == null)
            {
                return NotFound();
            }

            return RedirectToAction("Specializations", "Chefs", new { id = chef.ChefId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveSpecialization([Bind("PairId,ChefId,SpecialisationId")] ChefsCpecialisation tempChefsCpecialisation)
        {
            int id = (from cc in _context.ChefsCpecialisations
                      where cc.ChefId == tempChefsCpecialisation.ChefId && cc.SpecialisationId == tempChefsCpecialisation.SpecialisationId
                      select cc.PairId).FirstOrDefault();
            ChefsCpecialisation chefsCpecialisation = await _context.ChefsCpecialisations.FindAsync(id);
            if (ModelState.IsValid)
            {
                _context.ChefsCpecialisations.Remove(chefsCpecialisation);
                await _context.SaveChangesAsync();
            }

            if (tempChefsCpecialisation == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs.FindAsync(chefsCpecialisation.ChefId);
            if (chef == null)
            {
                return NotFound();
            }

            return RedirectToAction("Specializations", "Chefs", new { id = chef.ChefId });
        }

        // POST: Chefs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ChefId,FirstName,MiddleName,LastName,BirthCityId")] Chef chef)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chef);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            return View(chef);
        }

        // GET: Chefs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs.FindAsync(id);
            if (chef == null)
            {
                return NotFound();
            }
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            return View(chef);
        }

        // POST: Chefs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ChefId,FirstName,MiddleName,LastName,BirthCityId")] Chef chef)
        {
            if (id != chef.ChefId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chef);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChefExists(chef.ChefId))
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
            ViewData["BirthCityId"] = new SelectList(_context.Cities, "CityId", "CityId", chef.BirthCityId);

            ViewData["BirthCityName"] = new SelectList(_context.Cities, "CityId", "Name", chef.BirthCityId);

            return View(chef);
        }

        // GET: Chefs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chef = await _context.Chefs
                .Include(c => c.BirthCity)
                .FirstOrDefaultAsync(m => m.ChefId == id);
            if (chef == null)
            {
                return NotFound();
            }

            return View(chef);
        }

        // POST: Chefs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var chef = await _context.Chefs.FindAsync(id);
                foreach(Specialisation cc in FindSpecialisations(id))
                {
                    int _id = (from _cc in _context.ChefsCpecialisations
                              where _cc.ChefId == chef.ChefId && _cc.SpecialisationId == cc.SpecialisationId
                              select _cc.PairId).FirstOrDefault();
                    var chefsCpecialisation = await _context.ChefsCpecialisations.FindAsync(_id);
                        _context.ChefsCpecialisations.Remove(chefsCpecialisation);
                        await _context.SaveChangesAsync();
                }
                _context.Chefs.Remove(chef);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ChefExists(int id)
        {
            return _context.Chefs.Any(e => e.ChefId == id);
        }
        public async Task<IActionResult> RestaurantDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Restaurants", new { id = restaurant.RestaurantId });
        }

        public List<Restaurant> FindRestaurants(int? chefID)
        {
            if (chefID == null)
                return null;
            
            return _context.Restaurants.Where(r => r.ChefId == chefID).ToList();
        }
        public List<Specialisation> FindSpecialisations(int? chefID)
        {
            if (chefID == null)
                return null;

            var chefSpecialisations =
                (from spec in _context.Specialisations
                where spec.ChefsCpecialisations.Any(chs => chs.ChefId == chefID)
                select spec).ToList();
            return chefSpecialisations;
        }
        public List<Menu> FindMenus(int? chefID)
        {
            if (chefID == null)
                return null;

            var chefMenus =
                (from menu in _context.Menus
                 where menu.BrandChefId == chefID
                 select menu).ToList();
            return chefMenus;
        }
    }
}

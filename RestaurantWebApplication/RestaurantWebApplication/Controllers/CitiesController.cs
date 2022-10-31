using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApplication;

namespace RestaurantWebApplication.Controllers
{
    public class CitiesController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public CitiesController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            List<Tuple<City, List<Restaurant>, List<Chef>>> citiesWithInfo = new List<Tuple<City, List<Restaurant>, List<Chef>>>();

            foreach(City c in _context.Cities)
            {
                citiesWithInfo.Add(new Tuple<City, List<Restaurant>, List<Chef>>(
                    c,
                    FindRestaurants(c.CityId),
                    FindChefs(c.CityId)));
            }

            ViewBag.CitiesWithInfo = citiesWithInfo;

            var dBRestaurantsContext = _context.Cities.Include(c => c.Country);
            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Cities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        /*
        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewBag.Valid = true;

            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryId");

            ViewData["CountryName"] = new SelectList(_context.Countries, "CountryId", "Name");

            return View();
        }
        */
        public IActionResult Create(bool IsValid = true, string ErrorMsg = null)
        {
            ViewBag.Valid = IsValid;
            ViewBag.ErrorMsg = ErrorMsg;

            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryId");

            ViewData["CountryName"] = new SelectList(_context.Countries, "CountryId", "Name");

            return View();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,CountryId,Name")] City city)
        {
            List<City> tcl = (from c in _context.Cities
                         where c.Name == city.Name && c.CountryId == city.CountryId
                         select c).ToList();
            if(tcl.Count != 0)
            {
                bool Valid = false;
                string ErrorMsg = "THIS CITY ALREADY EXIST IN THIS COUNTRY";
                return Create(Valid, ErrorMsg);
            }

            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryId", city.CountryId);

            ViewData["CountryName"] = new SelectList(_context.Countries, "CountryId", "Name", city.CountryId);
            
            return View(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryId", city.CountryId);
            return View(city);
        }


        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CityId,CountryId,Name")] City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "CountryId", "CountryId", city.CountryId);
            return View(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var city = await _context.Cities
                .Include(c => c.Country)
                .FirstOrDefaultAsync(m => m.CityId == id);
            if (city == null)
            {
                return NotFound();
            }

            return View(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }

        public List<Restaurant> FindRestaurants(int? cityID)
        {
            if (cityID == null)
                return null;

            var cityRestaurants =
                (from r in _context.Restaurants
                 where r.CityId == cityID
                 select r).ToList();
            return cityRestaurants;
        }
        public List<Chef> FindChefs(int? cityID)
        {
            if (cityID == null)
                return null;

            var cityChefs =
                (from c in _context.Chefs
                 where c.BirthCityId == cityID
                 select c).ToList();
            return cityChefs;
        }
    }
}

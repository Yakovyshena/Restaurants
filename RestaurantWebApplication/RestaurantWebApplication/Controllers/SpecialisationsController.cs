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
    public class SpecialisationsController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public SpecialisationsController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Specialisations
        public async Task<IActionResult> Index()
        {
            List<Tuple<Specialisation, List<Chef>>> specWithChefs = new List<Tuple<Specialisation, List<Chef>>>();

            foreach (Specialisation spec in _context.Specialisations)
            {
                specWithChefs.Add(new Tuple<Specialisation, List<Chef>>(
                    spec,
                    FindChefs(spec.SpecialisationId)));
            }

            ViewBag.SpecialisationsWithChefs = specWithChefs;

            return View(await _context.Specialisations.ToListAsync());
        }

        // GET: Specialisations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialisation = await _context.Specialisations
                .FirstOrDefaultAsync(m => m.SpecialisationId == id);
            if (specialisation == null)
            {
                return NotFound();
            }

            return View(specialisation);
        }

        // GET: Specialisations/Create
        public IActionResult Create(bool IsValid = true, string ErrorMsg = null)
        {
            ViewBag.Valid = IsValid;
            ViewBag.ErrorMsg = ErrorMsg;

            return View();
        }

        // POST: Specialisations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SpecialisationId,Name")] Specialisation specialisation)
        {
            List<Specialisation> ts = (from s in _context.Specialisations
                              where s.Name == specialisation.Name
                              select s).ToList();
            if (ts.Count != 0)
            {
                bool Valid = false;
                string ErrorMsg = "THIS SPECIALIZATION ALREADY EXIST";
                return Create(Valid, ErrorMsg);
            }

            if (ModelState.IsValid)
            {
                _context.Add(specialisation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialisation);
        }

        // GET: Specialisations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialisation = await _context.Specialisations.FindAsync(id);
            if (specialisation == null)
            {
                return NotFound();
            }
            return View(specialisation);
        }

        // POST: Specialisations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SpecialisationId,Name")] Specialisation specialisation)
        {
            if (id != specialisation.SpecialisationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(specialisation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpecialisationExists(specialisation.SpecialisationId))
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
            return View(specialisation);
        }

        // GET: Specialisations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialisation = await _context.Specialisations
                .FirstOrDefaultAsync(m => m.SpecialisationId == id);
            if (specialisation == null)
            {
                return NotFound();
            }

            return View(specialisation);
        }

        // POST: Specialisations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var specialisation = await _context.Specialisations.FindAsync(id);
            _context.Specialisations.Remove(specialisation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpecialisationExists(int id)
        {
            return _context.Specialisations.Any(e => e.SpecialisationId == id);
        }

        public List<Chef> FindChefs(int? SpecialisationID)
        {
            var chefs = (from c in _context.Chefs
                        where c.ChefsCpecialisations.Any(cc => cc.SpecialisationId == SpecialisationID)
                        select c).ToList();
            return chefs;
        }
    }
}

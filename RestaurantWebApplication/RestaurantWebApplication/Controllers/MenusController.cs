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
    public class MenusController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public MenusController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            List<Tuple<Menu, List<Restaurant>>> menusWithRestaurants = new List<Tuple<Menu, List<Restaurant>>>();

            foreach(Menu m in _context.Menus)
            {
                menusWithRestaurants.Add(new Tuple<Menu, List<Restaurant>>(
                    m,
                    FindRestaurants(m.MenuId)));
            }

            ViewBag.MenusWithRestaurants = menusWithRestaurants;

            var dBRestaurantsContext = _context.Menus.Include(m => m.BrandChef);
            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Menus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.BrandChef)
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.Restaurants = FindRestaurants(menu.MenuId);
            ViewBag.Dishes = FindDishes(menu.MenuId);

            return View(menu);
        }

        public async Task<IActionResult> Dishes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            ViewBag.Dishes = _context.Dishes.ToList();
            ViewBag.MenuDishes = FindDishes(menu.MenuId);

            string chefName = (from c in _context.Chefs
                        where menu.BrandChefId == c.ChefId
                        select c.FullName).FirstOrDefault();

            ViewBag.FullChefName = chefName;


            return View(menu);
        }

        // GET: Menus/Create
        public IActionResult Create()
        {
            ViewData["BrandChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId");
            return View();
        }

        // POST: Menus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,BrandChefId")] Menu menu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", menu.BrandChefId);
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDish([Bind("PairId,MenuId,DishId")] MenusDish menusDish)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menusDish);
                await _context.SaveChangesAsync();
            }

            if (menusDish == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(menusDish.MenuId);
            if (menu == null)
            {
                return NotFound();
            }

            return RedirectToAction("Dishes", "Menus", new { id = menu.MenuId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveDish([Bind("PairId,MenuId,DishId")] MenusDish tempMenusDish)
        {
            int id = (from md in _context.MenusDishes
                      where md.MenuId == tempMenusDish.MenuId && md.DishId == tempMenusDish.DishId
                      select md.PairId).FirstOrDefault();
            MenusDish menusDish = await _context.MenusDishes.FindAsync(id);
            if (ModelState.IsValid)
            {
                _context.MenusDishes.Remove(menusDish);
                await _context.SaveChangesAsync();
            }

            if (tempMenusDish == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(menusDish.MenuId);
            if (menu == null)
            {
                return NotFound();
            }

            return RedirectToAction("Dishes", "Menus", new { id = menu.MenuId });
        }

        // GET: Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            ViewData["BrandChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", menu.BrandChefId);
            return View(menu);
        }

        // POST: Menus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MenuId,BrandChefId")] Menu menu)
        {
            if (id != menu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuExists(menu.MenuId))
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
            ViewData["BrandChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", menu.BrandChefId);
            return View(menu);
        }

        // GET: Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menu = await _context.Menus
                .Include(m => m.BrandChef)
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // POST: Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var menu = await _context.Menus.FindAsync(id);
                foreach (Dish d in FindDishes(id))
                {
                    int _id = (from _md in _context.MenusDishes
                               where _md.MenuId == menu.MenuId && _md.DishId == d.DishId
                               select _md.PairId).FirstOrDefault();
                    var menusDishes = await _context.MenusDishes.FindAsync(_id);
                    _context.MenusDishes.Remove(menusDishes);
                    await _context.SaveChangesAsync();
                }
                _context.Menus.Remove(menu);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menus.Any(e => e.MenuId == id);
        }

        public async Task<IActionResult> ChefDetails(int? id)
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

            return RedirectToAction("Details", "Chefs", new { id = chef.ChefId });
        }
        public async Task<IActionResult> DishDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Dishes", new { id =dish.DishId });
        }
        
        public List<Restaurant> FindRestaurants(int? menuID)
        {
            if (menuID == null)
                return null;

            List<Restaurant> restaurants =
                (from rest in _context.Restaurants
                where rest.MenuId == menuID
                select rest).ToList();
            return restaurants;
        }

        public List<Dish> FindDishes(int? menuID)
        {
            if (menuID == null)
                return null;

            List<Dish> dishes =
                (from dish in _context.Dishes
                where dish.MenusDishes.Any(md => md.MenuId == menuID)
                select dish).ToList();
            return dishes;
        }
    }
}

#nullable disable
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace RestaurantWebApplication.Controllers
{
    public class RestaurantsController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public RestaurantsController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index()
        {
            var dBRestaurantsContext = _context.Restaurants.Include(r => r.Chef).Include(r => r.City).Include(r => r.IconicDish).Include(r => r.Menu);
            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Chef)
                .Include(r => r.City)
                .Include(r => r.IconicDish)
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: Restaurants/Create
        public IActionResult Create(bool IsValid = true, string ErrorMsg = null)
        {
            ViewBag.Valid = IsValid;
            ViewBag.ErrorMsg = ErrorMsg;

            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId");
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId");
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "DishId");
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId");

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName");
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name");
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name");

            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RestaurantId,Name,CityId,CorrectAdress,MainThemeDefenition,IconicDishId,ChefId,MenuId")] Restaurant restaurant)
        {
            List<Restaurant> tr = (from r in _context.Restaurants
                              where r.Name == restaurant.Name && 
                              r.CityId == restaurant.CityId &&
                              r.CorrectAdress == restaurant.CorrectAdress
                              select r).ToList();
            if (tr.Count != 0)
            {
                bool Valid = false;
                string ErrorMsg = "THIS RESTAURANT ALREADY EXIST IN THIS CITY BY THIS ADDRESS";
                return Create(Valid, ErrorMsg);
            }

            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "DishId", restaurant.IconicDishId);
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", restaurant.MenuId);

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "FullName", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "DishId", restaurant.IconicDishId);
            
            return View(restaurant);
        }

        // GET: Restaurants/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "DishId", restaurant.IconicDishId);
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", restaurant.MenuId);

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            return View(restaurant);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RestaurantId,Name,CityId,CorrectAdress,MainThemeDefenition,IconicDishId,ChefId,MenuId")] Restaurant restaurant)
        {
            if (id != restaurant.RestaurantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.RestaurantId))
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
            ViewData["ChefId"] = new SelectList(_context.Chefs, "ChefId", "ChefId", restaurant.ChefId);
            ViewData["CityId"] = new SelectList(_context.Cities, "CityId", "CityId", restaurant.CityId);
            ViewData["IconicDishId"] = new SelectList(_context.Dishes, "DishId", "DishId", restaurant.IconicDishId);
            ViewData["MenuId"] = new SelectList(_context.Menus, "MenuId", "MenuId", restaurant.MenuId);

            ViewData["ChefName"] = new SelectList(_context.Chefs, "ChefId", "FullName", restaurant.ChefId);
            ViewData["CityName"] = new SelectList(_context.Cities, "CityId", "Name", restaurant.CityId);
            ViewData["IconicDishName"] = new SelectList(_context.Dishes, "DishId", "Name", restaurant.IconicDishId);

            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurants
                .Include(r => r.Chef)
                .Include(r => r.City)
                .Include(r => r.IconicDish)
                .Include(r => r.Menu)
                .FirstOrDefaultAsync(m => m.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            _context.Restaurants.Remove(restaurant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurants.Any(e => e.RestaurantId == id);
        }


        // GET: Restaurants/Details/5
        public async Task<IActionResult> ChefDetails(int? id)
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

            return RedirectToAction("Details", "Chefs", new { id = restaurant.ChefId });
        }

        public async Task<IActionResult> MenuDetails(int? id)
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

            return RedirectToAction("Details", "Menus", new { id = menu.MenuId });
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

            return RedirectToAction("Details", "Dishes", new { id = dish.DishId });
        }
    }
}

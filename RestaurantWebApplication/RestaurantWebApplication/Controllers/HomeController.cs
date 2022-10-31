using Microsoft.AspNetCore.Mvc;
using RestaurantWebApplication.Models;
using System.Diagnostics;

namespace RestaurantWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult RestaurantsIndex()
        {
            return RedirectToAction("Index", "Restaurants");
        }
        public IActionResult ChefsIndex()
        {
            return RedirectToAction("Index", "Chefs");
        }
        public IActionResult DishesIndex()
        {
            return RedirectToAction("Index", "Dishes");
        }
        //-------------------
        public IActionResult CitiesIndex()
        {
            return RedirectToAction("Index", "Cities");
        }
        public IActionResult CountriesIndex()
        {
            return RedirectToAction("Index", "Countries");
        }        
        public IActionResult ProductsIndex()
        {
            return RedirectToAction("Index", "Products");
        }
        public IActionResult ProductTypesIndex()
        {
            return RedirectToAction("Index", "ProductTypes");
        }
        public IActionResult SpecialisationsIndex()
        {
            return RedirectToAction("Index", "Specialisations");
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DBRestaurantsContext _context;

        public ChartController(DBRestaurantsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var restaurants = _context.Restaurants.ToList();
            var country = _context.Countries.ToList();
            List<object> restCountry = new List<object>();
            restCountry.Add(new[] {"Country", "Amount of restaurants"});
            foreach (var c in country)
            {
                int cA = 0;
                List<City> cities = 
                    (from city_ in _context.Cities
                    where city_.CountryId == c.CountryId
                    select city_).ToList();
                foreach (var city in cities)
                {
                    cA += city.Restaurants.Count();
                }
                restCountry.Add(new object[] { c.Name, cA });
            }
            return new JsonResult(restCountry);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DBRestaurantsContext _context;

        public ValuesController(DBRestaurantsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var products = _context.Products.ToList();
            var types = _context.ProductTypes.ToList();
            List<object> prodTypes = new List<object>();
            prodTypes.Add(new[] { "Product type", "Amount of different products" });
            foreach (var t in types)
            {
                List<Product> prds =
                    (from p in _context.Products
                     where p.ProductId == t.ProductTypeId
                     select p).ToList();
                int cP = prds.Count();
                prodTypes.Add(new object[] { t.Name, cP });
            }
            return new JsonResult(prodTypes);
        }
    }
}

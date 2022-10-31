using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantWebApplication;

namespace RestaurantWebApplication.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBRestaurantsContext _context;

        public ProductsController(DBRestaurantsContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            List<Tuple<Product, List<Dish>>> productWithDishes = new List<Tuple<Product, List<Dish>>>();

            foreach (Product p in _context.Products)
            {
                productWithDishes.Add(new Tuple<Product, List<Dish>>(
                    p,
                    FindDishes(p.ProductId)));
            }

            ViewBag.ProductWithDishes = productWithDishes;

            var dBRestaurantsContext = _context.Products.Include(p => p.ProductType);
            return View(await dBRestaurantsContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create(bool IsValid = true, string ErrorMsg = null)
        {
            ViewBag.Valid = IsValid;
            ViewBag.ErrorMsg = ErrorMsg;

            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId");

            ViewData["ProductTypeName"] = new SelectList(_context.ProductTypes, "ProductTypeId", "Name");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductTypeId")] Product product)
        {
            List<Product> tp = (from p in _context.Products
                              where p.Name == product.Name
                              select p).ToList();
            if (tp.Count != 0)
            {
                bool Valid = false;
                string ErrorMsg = "THIS PRODUCT ALREADY EXIST";
                return Create(Valid, ErrorMsg);
            }

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);

            ViewData["ProductTypeName"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeName", product.ProductTypeId);

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductTypeId")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "ProductTypeId", "ProductTypeId", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }

        public List<Dish> FindDishes(int? productID)
        {
            List<Dish> dishes = (from d in _context.Dishes
                                where d.DishesProducts.Any(dp => dp.ProductId == productID)
                                select d).ToList();
            return dishes;
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову
                                ProductType newType;
                                var t = (from pt in _context.ProductTypes
                                         where pt.Name.Contains(worksheet.Name)
                                         select pt).ToList();
                                if (t.Count > 0)
                                {
                                    newType = t[0];
                                }
                                else
                                {
                                    newType = new ProductType();
                                    newType.Name = worksheet.Name;
                                    //додати в контекст
                                    _context.ProductTypes.Add(newType);
                                }
                                //перегляд усіх рядків                    
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Product prod = new Product();
                                        prod.Name = row.Cell(1).Value.ToString();
                                        prod.ProductType = newType;

                                        var _prod = (from pd in _context.Products
                                                     where pd.Name == prod.Name
                                                     select pd).ToList();
                                        if(_prod.Count == 0)
                                            _context.Products.Add(prod);
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)

                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var types = _context.ProductTypes.ToList();
                //тут, для прикладу ми пишемо усі книжки з БД, в своїх проєктах ТАК НЕ РОБИТИ (писати лише вибрані)
                foreach (var t in types)
                {
                    var worksheet = workbook.Worksheets.Add(t.Name);

                    worksheet.Cell("A1").Value = "NAME";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var products = (from p in _context.Products
                                   where p.ProductTypeId == t.ProductTypeId
                                   select p).ToList();

                    //нумерація рядків/стовпчиків починається з індекса 1 (не 0)
                    for (int i = 0; i < products.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = products[i].Name;
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();

                    return new FileContentResult(stream.ToArray(),
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        //змініть назву файла відповідно до тематики Вашого проєкту

                        FileDownloadName = $"Products_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }

    }
}

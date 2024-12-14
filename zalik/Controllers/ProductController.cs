using Microsoft.AspNetCore.Mvc;
using zalik.Models;

namespace zalik.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public IActionResult Index()
        {
            var products = _dbContext.Products.ToList();
            return View(products);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            Console.WriteLine("=== Product creation started ===");
            Console.WriteLine($"Received Product Data: Name={product.Name}, Price={product.Price}, Quantity={product.Quantity}");

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState is valid");
                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                Console.WriteLine("Product added and saved to database");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                Console.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation error: {error.ErrorMessage}");
                }
            }

            return View(product);
        }



        public IActionResult Details(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        public IActionResult Edit(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Products.Update(product);
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        public IActionResult Delete(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _dbContext.Products.Find(id);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

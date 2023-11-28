using ALLUP2.DAL;
using ALLUP2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ALLUP2.Areas.Admin.Controllers
{
    [Area("admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product Product)
        {
          
            ViewBag.Tags = _context.Tags.ToList();
            if (!ModelState.IsValid)
            {

                return View();
            }
           
            bool check = true;
            if (Product.TagIds != null)
            {
                foreach (var item in Product.TagIds)
                {
                    if (!_context.Tags.Any(x => x.Id == item))
                    {
                        check = false;
                        break;
                    }
                }
            }
            if (check)
            {
                foreach (var item in Product.TagIds)
                {
                    ProductTag ProductTag = new ProductTag()
                    {
                        Product = Product,
                        TagId = item,
                    };
                    _context.ProductTags.Add(ProductTag);
                }
            }
            else
            {
                ModelState.AddModelError("TagId", "Error");
                return View();
            }

            _context.Products.Add(Product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int id)
        {
            
            ViewBag.Tags = _context.Tags.ToList();

            Product existProduct = _context.Products.Include(x => x.ProductTags).FirstOrDefault(x => x.Id == id);
            if (existProduct == null) return NotFound();

            existProduct.TagIds = existProduct.ProductTags.Select(x => x.TagId).ToList();
            return View(existProduct);


        }
        [HttpPost]
        public IActionResult Update(Product Product)
        {
           
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }
            Product existProduct = _context.Products.Include(x => x.ProductTags).FirstOrDefault(x => x.Id == Product.Id);
            if (existProduct == null) return NotFound();


            existProduct.ProductTags.RemoveAll(x => !Product.TagIds.Any(y => y == x.TagId));

            foreach (var item in Product.TagIds.Where(x => !existProduct.ProductTags.Any(y => y.TagId == x)))
            {
                ProductTag ProductTag = new ProductTag()
                {
                    TagId = item,
                };
                _context.ProductTags.Add(ProductTag);
            }

            existProduct.Name = Product.Name;
            existProduct.Description = Product.Description;
            existProduct.Costprice = Product.Costprice;
            existProduct.Saleprice = Product.Saleprice;
            existProduct.DiscountPercent = Product.DiscountPercent;
           

            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

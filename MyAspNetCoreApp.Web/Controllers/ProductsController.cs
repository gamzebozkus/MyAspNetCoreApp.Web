using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyAspNetCoreApp.Web.Helpers;
using MyAspNetCoreApp.Web.Models;
using MyAspNetCoreApp.Web.ViewModels;

namespace MyAspNetCoreApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ProductRepository _productRepository;

        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _productRepository = new ProductRepository();

            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {  
            var products = _context.Products.ToList();

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }
        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);
            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Add()
        {
            
            ViewBag.Expire = new Dictionary<string,int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
            new(){Data="Mavi", Value="Mavi"},
            new(){Data="Sarı",Value="Sarı"},
            new(){Data="Kırmızı",Value="Kırmızı"},
            new(){Data="Yeşil",Value="Yeşil"},


            }, "Value", "Data");
            return View();
        }

        [HttpPost]
        public IActionResult Add(Product newProduct )
        {
            //1.yöntem
          //var name=HttpContext.Request.Form["Name"].ToString();
          //var price=decimal.Parse( HttpContext.Request.Form["Price"].ToString());
          //var stock=int.Parse( HttpContext.Request.Form["Stock"].ToString());
          //var color=HttpContext.Request.Form["Color"].ToString();

            //2.yontem add parantez ıcınde parametre yazarız
           // Product newProduct = new Product() { Name = Name, Price = Price, Stock = Stock, Color = Color };


            _context.Products.Add(newProduct);
            _context.SaveChanges();
            TempData["status"] = "Ürün başarıyla eklendi.";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
           

           
            var product= _context.Products.Find(id);


            ViewBag.ExpireValue = product.Expire;
            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Ay",1 },
                {"3 Ay",3 },
                {"6 Ay",6 },
                {"12 Ay",12 }
            };


            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
            new(){Data="Mavi", Value="Mavi"},
            new(){Data="Sarı",Value="Sarı"},
            new(){Data="Kırmızı",Value="Kırmızı"},
            new(){Data="Yeşil",Value="Yeşil"},


            }, "Value", "Data", product.Color);
            return View(product);

        }
        [HttpPost]
        public IActionResult Update(Product updateProduct,int productId,string type)
        {
            updateProduct.Id = productId;
            _context.Products.Update(updateProduct);
            _context.SaveChanges();

            TempData["status"] = "Ürün başarıyla güncellendi ";
            return RedirectToAction("Index");

        }
    }
}

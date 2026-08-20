using System.Linq;
using CartMVCApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace CartMVCApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Tüm ürünleri listele
        public IActionResult Index()
        {
            var urunler = _context.Products.ToList();
            return View(urunler);
        }

        // Ürün detayı
        public IActionResult Details(int id)
        {
            var urun = _context.Products.FirstOrDefault(p => p.Id == id);
            if (urun == null)
                return NotFound();

            return View(urun);
        }
    }
}

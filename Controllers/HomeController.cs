using System.Diagnostics;
using System.Linq;
using CartMVCApp.Data;
using CartMVCApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CartMVCApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Ana sayfada öne çıkan birkaç ürünü göster
            var urunler = _context.Products.Take(3).ToList();
            return View(urunler);
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
    }
}

using System.Collections.Generic;
using System.Linq;
using CartMVCApp.Data;
using CartMVCApp.Extensions;
using CartMVCApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CartMVCApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string SEPET_KEY = "Sepet";

        public CartController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private List<CartItem> SepetiGetir()
        {
            return HttpContext.Session.GetObjectFromJson<List<CartItem>>(SEPET_KEY) ?? new List<CartItem>();
        }

        private void SepetiKaydet(List<CartItem> sepet)
        {
            HttpContext.Session.SetObjectAsJson(SEPET_KEY, sepet);
        }

        // ---------- SEPETİ GÖRÜNTÜLE ----------
        public IActionResult Index()
        {
            var sepet = SepetiGetir();
            return View(sepet);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int productId, int quantity = 1)
        {
            var urun = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (urun == null)
                return NotFound();
            var sepet = SepetiGetir();
            var mevcutUrun = sepet.FirstOrDefault(x => x.ProductId == productId);

            if (mevcutUrun != null)
            {
                mevcutUrun.Quantity += quantity;
            }
            else
            {
                sepet.Add(new CartItem
                {
                    ProductId = urun.Id,
                    ProductName = urun.Name,
                    Price = urun.Price,
                    Quantity = quantity,
                    ImageUrl = urun.ImageUrl
                });
            }

            SepetiKaydet(sepet);
            TempData["Mesaj"] = urun.Name + " sepete eklendi.";
            return RedirectToAction("Index", "Products");
        }

        // ---------- SEPETTEN ÜRÜN ÇIKAR ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int productId)
        {
            var sepet = SepetiGetir();
            var urun = sepet.FirstOrDefault(x => x.ProductId == productId);
            if (urun != null)
            {
                sepet.Remove(urun);
                SepetiKaydet(sepet);
            }

            return RedirectToAction("Index");
        }

        // ---------- MİKTAR GÜNCELLE ----------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var sepet = SepetiGetir();
            var urun = sepet.FirstOrDefault(x => x.ProductId == productId);
            if (urun != null && quantity > 0)
            {
                urun.Quantity = quantity;
                SepetiKaydet(sepet);
            }

            return RedirectToAction("Index");
        }

        // ---------- SİPARİŞİ ONAYLAMA SAYFASI ----------
        [Authorize]
        public IActionResult Checkout()
        {
            var sepet = SepetiGetir();
            if (!sepet.Any())
                return RedirectToAction("Index");

            return View(sepet);
        }

        // ---------- SİPARİŞİ ONAYLA (VERİTABANINA KAYDET) ----------
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<IActionResult> ConfirmOrder(string teslimatAdresi)
        {
            var sepet = SepetiGetir();
            if (!sepet.Any())
                return RedirectToAction("Index");

            var kullanici = await _userManager.GetUserAsync(User);

            var siparis = new Order
            {
                UserId = kullanici.Id,
                TeslimatAdresi = teslimatAdresi,
                TotalPrice = sepet.Sum(x => x.Toplam),
                Items = sepet.Select(x => new OrderItem
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Quantity = x.Quantity
                }).ToList()
            };

            _context.Orders.Add(siparis);
            await _context.SaveChangesAsync();

            // Sepeti temizle
            HttpContext.Session.Remove(SEPET_KEY);

            return RedirectToAction("OrderConfirmation", new { id = siparis.Id });
        }

        // ---------- SİPARİŞ ONAY SAYFASI ----------
        [Authorize]
        public IActionResult OrderConfirmation(int id)
        {
            var siparis = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == id);
            if (siparis == null)
                return NotFound();

            return View(siparis);
        }
    }
}

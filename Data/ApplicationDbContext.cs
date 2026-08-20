using CartMVCApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CartMVCApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Örnek ürünler (ilk çalıştırmada veritabanına eklenir)
            builder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Kablosuz Kulaklık", Description = "Bluetooth 5.0, gürültü engelleme özellikli kulaklık.", Price = 799.90m, Stock = 50, ImageUrl = "/images/kulaklik.svg" },
                new Product { Id = 2, Name = "Akıllı Saat", Description = "Nabız ölçer ve adım sayar özellikli akıllı saat.", Price = 1299.00m, Stock = 30, ImageUrl = "/images/akillisaat.svg" },
                new Product { Id = 3, Name = "Mekanik Klavye", Description = "RGB aydınlatmalı, mavi switch mekanik klavye.", Price = 649.50m, Stock = 40, ImageUrl = "/images/klavye.svg" },
                new Product { Id = 4, Name = "Kablosuz Mouse", Description = "Ergonomik tasarımlı, sessiz tıklamalı mouse.", Price = 249.90m, Stock = 100, ImageUrl = "/images/mouse.svg" },
                new Product { Id = 5, Name = "USB-C Hub", Description = "7 in 1 çoklu bağlantı adaptörü.", Price = 349.00m, Stock = 60, ImageUrl = "/images/usbhub.svg" },
                new Product { Id = 6, Name = "Taşınabilir SSD 1TB", Description = "Yüksek hızlı harici depolama birimi.", Price = 1899.00m, Stock = 20, ImageUrl = "/images/ssd.svg" }
            );
        }
    }
}

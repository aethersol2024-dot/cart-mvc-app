using System;
using CartMVCApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CartMVCApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // Uygulama ilk kez ayağa kalktığında veritabanını ve
            // örnek ürünleri (seed data) otomatik olarak oluşturur.
            // Bu sayede hosting ortamında (örn. Render.com) elle
            // migration komutu çalıştırmaya gerek kalmaz.
            using (var scope = host.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Veritabanı oluşturulurken hata oluştu.");
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    // Render.com (ve benzeri platformlar) PORT ortam değişkeniyle
                    // hangi porta bağlanılması gerektiğini bildirir.
                    var port = Environment.GetEnvironmentVariable("PORT");
                    if (!string.IsNullOrEmpty(port))
                    {
                        webBuilder.UseUrls($"http://0.0.0.0:{port}");
                    }

                    webBuilder.UseStartup<Startup>();
                });
    }
}

using Microsoft.AspNetCore.Identity;

namespace CartMVCApp.Models
{
    // Identity kullanıcı sınıfı - istenirse ek alanlar (Ad, Soyad vb.) buraya eklenebilir
    public class ApplicationUser : IdentityUser
    {
        public string AdSoyad { get; set; }
    }
}

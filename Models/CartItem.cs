using System;

namespace CartMVCApp.Models
{
    // Bu sınıf veritabanına değil, oturum (Session) içine JSON olarak kaydedilir
    [Serializable]
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }

        public decimal Toplam => Price * Quantity;
    }
}

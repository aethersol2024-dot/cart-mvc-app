using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CartMVCApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        public string TeslimatAdresi { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

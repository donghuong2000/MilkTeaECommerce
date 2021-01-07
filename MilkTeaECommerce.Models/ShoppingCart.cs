using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class ShoppingCart
    {
        public string Id { get; set; }
        public string ApplicationUserId { get; set; }
        public string ProductId { get; set; }
        public int? Count { get; set; }
        public float? Price { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Product Product { get; set; }
    }
}

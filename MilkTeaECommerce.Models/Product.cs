using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ProductDiscount = new HashSet<ProductDiscount>();
            Ratings = new HashSet<Rating>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float? Price { get; set; }
        public string Status { get; set; }
        public int? Quantity { get; set; }
        public string CategoryId { get; set; }
        public string ShopId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscount { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}

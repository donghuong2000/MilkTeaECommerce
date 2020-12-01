using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTeaECommerce.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            OrderHeaders = new HashSet<OrderHeader>();
            Ratings = new HashSet<Rating>();
            ShoppingCarts = new HashSet<ShoppingCart>();
        }

        public string  Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }

        [NotMapped]
        public string Role { get; set; }
        public virtual Shop Shops { get; set; }
        
        public virtual ICollection<OrderHeader> OrderHeaders { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
    }
}

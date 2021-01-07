using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class Category
    {
        public Category()
        {
            CategoryDiscount = new HashSet<CategoryDiscount>();
            Products = new HashSet<Product>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CategoryDiscount> CategoryDiscount { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}

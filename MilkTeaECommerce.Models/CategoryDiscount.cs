using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class CategoryDiscount
    {
        public string DiscountId { get; set; }
        public string CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Discount Discount { get; set; }
    }
}

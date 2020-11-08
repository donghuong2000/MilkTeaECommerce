using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class ProductDiscount
    {
        public string DiscountId { get; set; }
        public string ProductId { get; set; }

        public virtual Discount Discount { get; set; }
        public virtual Product Product { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class Discount
    {
        public Discount()
        {
            CategoryDiscount = new HashSet<CategoryDiscount>();
            DeliveryDiscount = new HashSet<DeliveryDiscount>();
            ProductDiscount = new HashSet<ProductDiscount>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateExpired { get; set; }
        public int? TimesUsed { get; set; }
        public int? TimesUseLimit { get; set; }
        public int? PercentDiscount { get; set; }
        public float? MaxDiscount { get; set; }
        public string Code { get; set; }

        public virtual ICollection<CategoryDiscount> CategoryDiscount { get; set; }
        public virtual ICollection<DeliveryDiscount> DeliveryDiscount { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscount { get; set; }
    }
}

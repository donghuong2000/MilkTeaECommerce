using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class DeliveryDiscount
    {
        public string DiscountId { get; set; }
        public string DeliveryId { get; set; }

        public virtual Delivery Delivery { get; set; }
        public virtual Discount Discount { get; set; }
    }
}

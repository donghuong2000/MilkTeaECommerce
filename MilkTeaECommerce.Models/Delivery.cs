using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class Delivery
    {
        public Delivery()
        {
            DeliveryDetails = new HashSet<DeliveryDetail>();
            DeliveryDiscount = new HashSet<DeliveryDiscount>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DeliveryDetail> DeliveryDetails { get; set; }
        public virtual ICollection<DeliveryDiscount> DeliveryDiscount { get; set; }
    }
}

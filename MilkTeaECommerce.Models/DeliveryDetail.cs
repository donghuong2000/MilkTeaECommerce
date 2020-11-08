using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class DeliveryDetail
    {
        public string OrderDetailId { get; set; }
        public string DeliveryId { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public float? Price { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public virtual Delivery Delivery { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
    }
}

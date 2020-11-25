using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkTeaECommerce.Models
{
    public partial class DeliveryDetail
    {
        
        public string OrderDetailId { get; set; }
        public string DeliveryId { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public float? Price { get; set; } // null able type
        public DateTime? DateStart { get; set; } // null able type
        public DateTime? DateEnd { get; set; } // null able type

        public virtual Delivery Delivery { get; set; }
        public virtual OrderDetail OrderDetail { get; set; }
    }
}

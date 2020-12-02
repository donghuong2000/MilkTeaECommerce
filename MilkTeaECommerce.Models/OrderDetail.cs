using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace MilkTeaECommerce.Models
{
    public partial class OrderDetail
    {
        
        public string Id { get; set; }
        public string OrderHeaderId { get; set; }
        public string ProductId { get; set; }
        public int? Count { get; set; }
        public float? Price { get; set; }
        public string Status { get; set; }
        public string ShipperId { get; set; }
        [ForeignKey("ShipperId")]
        public ApplicationUser Shipper { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
        public virtual Product Product { get; set; }
        public virtual DeliveryDetail DeliveryDetails { get; set; }
        
    }
}

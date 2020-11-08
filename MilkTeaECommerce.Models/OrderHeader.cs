using System;
using System.Collections.Generic;

namespace MilkTeaECommerce.Models
{
    public partial class OrderHeader
    {
        public OrderHeader()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string Id { get; set; }
        public string ApplicationUserId { get; set; }
        public float? Price { get; set; }
        public string PaymentStatus { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}

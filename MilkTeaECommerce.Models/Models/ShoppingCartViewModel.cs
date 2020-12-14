using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class ShoppingCartViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        public Delivery Delivery { get; set; }
    }
}

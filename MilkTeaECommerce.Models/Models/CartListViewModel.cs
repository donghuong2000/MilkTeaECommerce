using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class CartItemViewModel
    {

        public string cmd { get; set; }
        public string add { get; set; }
        public float amount { get; set; }
        public string discount_amount { get; set; }

        public string href { get; set; }

        public string item_name { get; set; }

        public int quantity { get; set; }

        public string shipping { get; set; }

        public string submit { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public long price { get; set; }
        public string description { get; set; }
    }
}

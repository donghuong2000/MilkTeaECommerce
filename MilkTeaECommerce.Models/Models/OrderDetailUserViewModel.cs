    using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class OrderDetailUserViewModel
    {
       
        public string shopimage { get; set; }
        public string shopid { get; set; }
        public string shopname { get; set; }
        public string productimage { get; set; }
        public string productid { get; set; }
        public string productname { get; set; }
        public string categoryname { get; set; }
        public int? count  { get; set; }
        public string deliveryandprice { get; set; }
        public string status { get; set; }
        public float? beforeprice { get; set; }
        public float? afterprice { get; set; }
        public float? totalprice { get; set; }
        public string orderdetailid { get; set; }
    }
}

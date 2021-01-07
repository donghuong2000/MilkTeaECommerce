using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
     public class ShopProfileViewModel
    {
        public string ApplicationUserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImgUrl { get; set; }
        public IFormFile File { get; set; }
    }
}

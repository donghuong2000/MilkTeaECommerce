using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class ProductViewModel
    {
 
        public string Id { get; set; }
        [Required(ErrorMessage = "Dữ liệu không hợp lệ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Dữ liệu không hợp lệ")]
        [StringLength(maximumLength:1000,ErrorMessage ="Nội dung miêu tả tối đa 1000 kí tự")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Dữ liệu không hợp lệ")]
        [Range(4, 1000000, ErrorMessage = "Dữ liệu không hợp lệ")]

        public float? Price { get; set; }
        [Required(ErrorMessage = "Dữ liệu không hợp lệ")]
        public bool IsConfirm { get; set; }
        [Required(ErrorMessage = "Dữ liệu không hợp lệ")]
        [Range(minimum:10,maximum:1000000, ErrorMessage = "Dữ liệu không hợp lệ")]
        public int? Quantity { get; set; }
        public string CategoryId { get; set; }
        public string ImageUrl { get; set; }
        
        public IFormFile files { get; set; }

    }
}
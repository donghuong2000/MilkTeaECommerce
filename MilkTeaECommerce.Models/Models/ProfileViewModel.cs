using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class ProfileViewModel
    {
        [Display(Name = "Tên tài khoản")]
        public string Username { get; set; }
        [Display(Name = "Tên người dùng")]
        public string Name { get; set; }
        
        public string Email { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }
        public bool IsMailComfirm { get; set; }
        [Display(Name = "Số điện thoại")]
        public string Phonenumber { get; set; }
        public bool IsPhoneComfirm { get; set; }
        public string ImageUrl { get; set; }
        [Display(Name = "Ảnh đại diện")]
        public IFormFile file { get; set; }
    }
}

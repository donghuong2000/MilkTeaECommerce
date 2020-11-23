using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MilkTeaECommerce.Models.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }    
        public string Phonenumber { get; set; }

        public IFormFile ImageUrl { get; set; }
    }
}

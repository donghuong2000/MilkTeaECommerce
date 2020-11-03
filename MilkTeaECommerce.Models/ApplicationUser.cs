using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using System.Text;

namespace MilkTeaECommerce.Models
{
    public class ApplicationUser: IdentityUser
    {


        [NotMapped]
        public string Role { get; set; }
    }
}

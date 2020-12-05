using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MilkTeaECommerce.Models.Models;
using Newtonsoft.Json.Linq;

namespace MilkTeaECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
    }
}

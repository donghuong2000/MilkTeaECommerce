using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MilkTeaECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Detail(string a)
        {
            return View();
        }
    }
}

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
        [HttpGet]
        public IActionResult Detail()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Detail(string cart)
        {
            var obj = JArray.Parse(cart);
            var cartItem = obj.Select(x=>x.ToObject<CartItemViewModel>());

            return View(cartItem);
        }
    }
}

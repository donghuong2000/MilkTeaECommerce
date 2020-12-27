using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
   //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var listshop = _db.Shops.ToList();

            ViewData["listshop"] = _db.Shops.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.ApplicationUserId
            }).ToList();
            // duy code
            var lsShop = _db.Shops.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.ApplicationUserId
            }).ToList();
            return View(lsShop);
        }
        public IActionResult GetData(string Id)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {
            //var totalShop = _db.Shops.Include(x => x.Products).Where(x => x.ApplicationUserId == Id);

            var orderCount = _db.OrderDetails
                .Include(x => x.Product)
                .Where(x => x.Product.ShopId == Id)
                .Select(x => new { 
                    product = x.Product.Name,
                    price = x.Price
                })
                .GroupBy(x=>x.product)
                .Select(x=> new {
                    Key = x.Key,
                    Value = x.Sum(s => s.price)    
                }).ToList();
            float SumPrice = 0;
            var a = new List<string>();
            var b = new List<float>();
            foreach (var item in orderCount)
            {
                SumPrice += (float)item.Value;
                a.Add(item.Key);
                b.Add((float)item.Value);
            }
           
            TempData["Earningsmonth"] = SumPrice.ToString();
            return Json(new { a, b});


                
            //var totalProduct = _db.OrderDetails.Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id && x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).ToString();
            //return NotFound();
        }
    }
}

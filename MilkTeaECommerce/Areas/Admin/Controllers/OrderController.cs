using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Getall()
        {
            var obj = _db.OrderDetails.Include(x=>x.DeliveryDetails).ThenInclude(x=>x.Delivery)
                .Include(x=>x.Product)
                .Select(x=>new { 
                    id = x.Id,
                    quantity = x.Count,
                    image = x.Product.ImageUrl,
                    product = x.Product.Name,
                    price = x.Price,
                    status = x.Status,
                    delivery = x.DeliveryDetails.Delivery.Name +" : "+ x.DeliveryDetails.Price
                }).ToList();
            return Json(new {data = obj });
        }

    }
}

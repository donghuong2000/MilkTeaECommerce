using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;

namespace MilkTeaECommerce.Areas.Seller.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly IHostEnvironment _hostEnvironment;

        public ProductController(ApplicationDbContext db, IHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var list = _db.Products
                .Include(x => x.Category)
                .Where(x => x.ShopId == claim.Value)
                .Select(x => new
                {
                    id = x.Id,
                    image = x.ImageUrl,
                    de = x.Description,
                    price = x.Price,
                    quan = x.Quantity,
                    category = x.Category.Name,
                    confirm = x.IsConfirm

                }) ;
            return Json(new {data = list });
        }
    }
}

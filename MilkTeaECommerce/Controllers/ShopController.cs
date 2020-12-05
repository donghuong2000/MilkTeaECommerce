using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // test id shop
            var idShop = _context.Shops.Select(x => x.ApplicationUserId).ToList();

            ViewBag.IdShop = idShop;
            return View();
        }
        [HttpGet]
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Shop shop = new Shop();
            shop = _context.Shops.Include(x=>x.ApplicationUser).Where(x => x.ApplicationUserId == id).SingleOrDefault();
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }

        // api
        [HttpGet]
        public IActionResult getshop(string id)
        {
            var shop = _context.Shops.Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == id).Select(x => new
            {
                Id=x.ApplicationUserId,
                Name=x.Name,
                Des=x.Description,
                Image=x.ImgUrl,
                Address=x.ApplicationUser.Address
            }).SingleOrDefault();
            return Json(shop);
        }
        [HttpGet]
        public IActionResult getproducts(string idShop)
        {
            var products = _context.Products.Where(x => x.ShopId == idShop && x.IsConfirm== true).Select(x => new
            {
                Id=x.Id,
                Name=x.Name,
                Price=x.Price.GetValueOrDefault().ToString("#,###"),
                Image=x.ImageUrl
            });
            return Json(products);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public ShopController(ApplicationDbContext db)
        {
            
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var obj = _db.Shops.Include(x => x.Products).Select(x=>new { 
                id = x.ApplicationUserId,
                image = x.ImgUrl,
                name = x.Name,
                de = x.Description,
                count = x.Products.Count,
                isban = x.IsConfirm
            
            });
            return Json(new { data = obj });
        }

        public IActionResult LockUnLock(string id)
        {
            try
            {
                var obj = _db.Shops.Find(id);
                obj.IsConfirm = !obj.IsConfirm;
                _db.SaveChanges();
                return Json(new { success = true, message = "Shop has been Ban/UnBan" });

            }
            catch (Exception e )
            {

                return Json(new { success = false, message = e.Message });
            }
            
            

        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;

namespace MilkTeaECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Detail(string id)
        {
            if(id == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var product = _db.Products.Include(x=>x.Category).Include(x=>x.Shop).FirstOrDefault(x => x.Id == id);
                if(product!=null)
                {
                    return View(product);
                }
                return NotFound();
            }
            
        }
    }
}

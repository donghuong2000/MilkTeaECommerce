using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public IActionResult Index()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "I");
            return View();
        }
        public IActionResult GetAll()
        {
            var list = _context.Products.Include(x=>x.Category).Include(x => x.Shop).Select(x=>new { 
                id = x.Id,
                name = x.Name,
                de = x.Description==null? "0 Kí tự" : x.Description.Length + " Kí tự",
                price = x.Price,
                quantity = x.Quantity,
                shop = x.Shop.Name,
                category = x.Category.Name,
                image = x.ImageUrl,
                isConfirm = x.IsConfirm
            }).ToList();
            return Json(new { data = list });
        }
       
        
        [HttpPost]
  
        public IActionResult Create(string id, string name, string description, string imageUrl, float price,string status, int quantity, string categoryId, string shopId)
        {
            try
            {
                Product product = new Product() { Id = id, Name = name, Description = description, ImageUrl = imageUrl, Price = price,Quantity = quantity, CategoryId = categoryId, ShopId = shopId };
                _context.Products.Add(product);
                _context.SaveChanges();
                
                return Json(new { success = true, message = "khởi tạo thành công danh mục" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public IActionResult Get(string id)
        {
            var product = _context.Products.FirstOrDefault(x => x.Id == id);

            var obj = new
            {
                id = product.Id,
                name = product.Name,
                description = product.Description,
                imageurl = product.ImageUrl,
                price = product.Price,
                //status = product.Status,
                quantity = product.Quantity,
                categoryId = product.CategoryId,
                shopId = product.ShopId,
                

            };
            return Json(new { data = obj });
        }


        public IActionResult LockUnLock(string id)
        {
            try
            {
                var obj = _context.Products.Find(id);
                obj.IsConfirm = !obj.IsConfirm;
                _context.Products.Update(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "khóa/mở khóa thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var obj = _context.Products.Find(id);
                _context.Products.Remove(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

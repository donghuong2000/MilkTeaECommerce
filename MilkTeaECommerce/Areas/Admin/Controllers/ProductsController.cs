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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            ViewData["ShopId"] = new SelectList(_context.Shops, "Id", "Id");
            return View();
        }
        public IActionResult GetAll()
        {
            var list = _context.Products.ToList();
            return Json(new { data = list });
        }
        public IActionResult GetforSelect(string q)
        {
            var obj = _context.Products.ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Status = x.Status,
                    Quantity = x.Quantity,
                    CategoryId = x.CategoryId,
                    ShopId = x.ShopId,

                });
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                obj = obj.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = obj });
        }
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Create(string id, string name, string description, string imageUrl, float price,string status, int quantity, string categoryId, string shopId)
        {
            try
            {
                Product product = new Product() { Id = id, Name = name, Description = description, ImageUrl = imageUrl, Price = price, Status = status,Quantity = quantity, CategoryId = categoryId, ShopId = shopId };
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
                status = product.Status,
                quantity = product.Quantity,
                categoryId = product.CategoryId,
                shopId = product.ShopId,
                

            };
            return Json(new { data = obj });
        }
        //GET: Admin/Products/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Json(new { data = product });
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Update(string oldid, string name, string description, string imageurl, float price, string status, int quantity, string categoryid, string shopid)
        {
            var obj = _context.Products.Find(oldid);
            obj.Name = name;
            obj.Description = description;
            obj.ImageUrl = imageurl;
            obj.Price = price;
            obj.Status = status;
            obj.Quantity = quantity;
            obj.CategoryId = categoryid;
            obj.ShopId = shopid;
            try
            {

                _context.Products.Update(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "cập nhập mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }
        [HttpPost]
        public IActionResult Edit(string id, [Bind("Id,Name,Description,ImageUrl,Price,Status,Quantity,CategoryId,ShopId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);
            ViewData["ShopId"] = new SelectList(_context.Shops, "ApplicationUserId", "ApplicationUserId", product.ShopId);
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpDelete]
        //[ValidateAntiForgeryToken]
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

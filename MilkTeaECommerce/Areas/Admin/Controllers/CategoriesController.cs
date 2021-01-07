using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var list = _context.Categories.ToList();
            return Json(new { data = list });
        }

        public IActionResult GetforSelect(string q)
        {
            var obj = _context.Categories.ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,

                });
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                obj = obj.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = obj });
        }
        
        [HttpPost]
        public IActionResult Create(string id, string name)
        {

            try
            {
                if (name == null || id == null)
                    throw new Exception("Không được để trống tên");
                if(_context.Categories.Select(x=>x.Name).Contains(name))
                    throw new Exception("Tên bị trùng bạn êy");
                Category category = new Category() { Id = id, Name = name};
                _context.Categories.Add(category);
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
            var product = _context.Categories.FirstOrDefault(x => x.Id == id);

            var obj = new
            {
                id = product.Id,
                name = product.Name,
                
            };
            return Json(new { data = obj });
        }
        // GET: Admin/Categories/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return Json(new { data = category });
        }


        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Update(string oldid, string name)
        {
            var obj = _context.Categories.Find(oldid);
            obj.Name = name;
           
            try
            {
                if (name == null )
                    throw new Exception("Không được để trống tên");
                if (_context.Categories.Select(x => x.Name).Contains(name))
                    throw new Exception("Tên bị trùng bạn êy");
                _context.Categories.Update(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "cập nhập mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        // GET: Admin/Categories/Delete/5
        

        // POST: Admin/Categories/Delete/5
        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                var obj = _context.Categories.Find(id);
                _context.Categories.Remove(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        private bool CategoryExists(string id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}

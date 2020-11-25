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
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Deliveries
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetAll()
        {
            var list = _context.Deliveries.ToList();
            return Json(new { data = list });
        }
        public IActionResult GetforSelect(string q)
        {
            var obj = _context.Deliveries.ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    Text = x.Name,

                });
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                obj = obj.Where(x => x.Text.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = obj });
        }
      

       

        // POST: Admin/Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
       
        public IActionResult Create(string id, string name)
        {
            try
            {
                Delivery delivery = new Delivery() { Id = id, Name = name};
                _context.Deliveries.Add(delivery);
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
            var delivery = _context.Deliveries.FirstOrDefault(x => x.Id == id);

            var obj = new
            {
                id = delivery.Id,
                name = delivery.Name,
                
            };
            return Json(new { data = obj });
        }

        [HttpPost]
        public IActionResult Update(string newid, string newname)
        {
            var obj = _context.Deliveries.Find(newid);
            obj.Name = newname;
            try
            {

                _context.Deliveries.Update(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "cập nhập mục thành công" });
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
                var obj = _context.Deliveries.Find(id);
                _context.Deliveries.Remove(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        } 
    }
}

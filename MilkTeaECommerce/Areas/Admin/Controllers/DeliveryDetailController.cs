using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DeliveryDetailController : Controller
    {
        
        private readonly ApplicationDbContext _context;

        public DeliveryDetailController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DeliveryDetail
        public IActionResult Index()
        {
            
            ViewBag.orderdetailid_list = new SelectList(_context.OrderDetails.ToList(),"Id","Id");
            ViewBag.deliveryid_list = new SelectList(_context.Deliveries.ToList(), "Id", "Name");
            return View();
        }


        public IActionResult GetAll()
        {
            var list = _context.DeliveryDetails.ToList()
                .Select(x => new
                {
                    orderdetailid = x.OrderDetailId,
                    deliveryid = x.DeliveryId,
                    address = x.Address,
                    note = x.Note,
                    price = x.Price,
                    datestart = x.DateStart.GetValueOrDefault().ToShortDateString(),
                    dateend = x.DateEnd.GetValueOrDefault().ToShortDateString(),
                });
            return Json(new { data = list });
        }
        public IActionResult GetforSelect(string q)
        {
            var obj = _context.DeliveryDetails.ToList()
                .Select(x => new
                {
                    orderdetailid = x.OrderDetailId,
                    deliveryid = x.DeliveryId,
                    address = x.Address,
                    note = x.Note,
                    price = x.Price,
                    datestart = x.DateStart.GetValueOrDefault().ToShortDateString(),
                    dateend = x.DateEnd

                });
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                obj = obj.Where(x => x.orderdetailid.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = obj });
        }




        // POST: Admin/Deliveries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public IActionResult Create(string orderdetailid, string deliveryid, string address, string note,string price, string datestart,string dateend)
        {
            try
            {
                DeliveryDetail deliverydetail = new DeliveryDetail() 
                { OrderDetailId = orderdetailid, DeliveryId = deliveryid, Address = address, Note = note, Price = float.Parse(price), DateStart = DateTime.Parse(datestart), DateEnd = DateTime.Parse(dateend)};
                _context.DeliveryDetails.Add(deliverydetail);
                _context.SaveChanges();
                return Json(new { success = true, message = "khởi tạo thành công chi tiết vận chuyển" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult Get(string id)
        {
            var DeliveryDetails = _context.DeliveryDetails.FirstOrDefault(x => x.OrderDetailId == id);

            var obj = new
            {
                orderDetailid = DeliveryDetails.OrderDetailId,
                deliveryId = DeliveryDetails.DeliveryId,
                address = DeliveryDetails.Address,
                note = DeliveryDetails.Note,
                price = DeliveryDetails.Price,
                dateStart = DeliveryDetails.DateStart.GetValueOrDefault().ToString("yyyy-MM-dd"),
                dateEnd = DeliveryDetails.DateEnd.GetValueOrDefault().ToString("yyyy-MM-dd")

            };
            return Json(new { data = obj });
        }

        [HttpPost]
        public IActionResult Update(string orderdetailid, string deliveryid,string address, string note,string price, string datestart, string dateend)
            {
            var obj = _context.DeliveryDetails.Find(orderdetailid);
            obj.DeliveryId = deliveryid;
            obj.Address = address;
            obj.Note = note;
            obj.Price = float.Parse(price);
            obj.DateStart = DateTime.Parse(datestart);
            obj.DateEnd = DateTime.Parse(dateend);
            try
            {

                _context.DeliveryDetails.Update(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "cập nhập chi tiết vận chuyển thành công" });
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
                var obj = _context.DeliveryDetails.Find(id);
                _context.DeliveryDetails.Remove(obj);
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


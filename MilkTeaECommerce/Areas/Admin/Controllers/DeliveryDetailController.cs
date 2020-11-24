using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            return View();
        }


        public IActionResult GetAll()
        {
            var list = _context.DeliveryDetails.ToList();
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
                    datestart = x.DateStart,
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

        public IActionResult Create(string orderdetailid, string deliveryid, string address, string note,float price, DateTime datestart,DateTime dateend)
        {
            try
            {
                DeliveryDetail deliverydetail = new DeliveryDetail() { OrderDetailId = orderdetailid, DeliveryId = deliveryid, Address = address, Note = note, Price = price, DateStart = datestart, DateEnd = dateend};
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
                orderdetailid = DeliveryDetails.OrderDetailId,
                deliveryId = DeliveryDetails.DeliveryId,
                address = DeliveryDetails.Address,
                note = DeliveryDetails.Note,
                price = DeliveryDetails.Price,
                dateStart = DeliveryDetails.DateStart,
                dateEnd = DeliveryDetails.DateEnd

            };
            return Json(new { data = obj });
        }

        [HttpPost]
        public IActionResult Update(string orderdetailid, string deliveryid,string address, string note,float price, DateTime datestart, DateTime dateend)
        {
            var obj = _context.DeliveryDetails.Find(orderdetailid);
            obj.DeliveryId = deliveryid;
            obj.Address = address;
            obj.Note = note;
            obj.Price = price;
            obj.DateStart = datestart;
            obj.DateEnd = dateend;
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


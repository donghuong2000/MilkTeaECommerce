using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Shipper
{
    [Area("Shipper")]
    public class DeliveryDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DeliveryDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult GetAll()
        {

            var orderHeader = _context.OrderDetails.Include(x=>x.OrderHeader).Where(x=>x.Status==null).Select(x => new
            {
                //id=x.Id,
                //orderHeaderId=x.OrderHeaderId,
                //productId=x.ProductId,
                //count=x.Count,
                //price=x.Price,
                //status=x.Status,
                headerId=x.OrderHeader.Id,
                price=x.OrderHeader.Price.GetValueOrDefault().ToString("#,###"),
                payment =x.OrderHeader.PaymentStatus,
                address=x.OrderHeader.Address,
                phone=x.OrderHeader.Phone,
                orderDetailId=x.Id,
                shop=x.OrderHeader.ApplicationUserId
            });
            return Json(new { data = orderHeader });
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.ShipperId = claim.Value;
            return View();
        }

        // GET: Shipper/DeliveryDetails/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deliveryDetail = await _context.DeliveryDetails
                .Include(d => d.Delivery)
                .Include(d => d.OrderDetail)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (deliveryDetail == null)
            {
                return NotFound();
            }

            return View(deliveryDetail);
        }

        [HttpPost]
        
        public IActionResult Get(string id)
        {
            // add deliveryId cho DeliveryDetail
            var orderDetail = _context.OrderDetails.Where(x => x.Id == id).SingleOrDefault();
            orderDetail.Status = "Đã nhận đơn";

            // add shipper id   + add vào shopping cartx    
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            orderDetail.ShipperId = claim.Value;
           
            try
            {
                _context.OrderDetails.Update(orderDetail);
                 _context.SaveChanges();
            }
            catch(Exception e)
            {

            }
            return Json(new { success = true });
        }

        private bool DeliveryDetailExists(string id)
        {
            return _context.DeliveryDetails.Any(e => e.OrderDetailId == id);
        }
        [HttpGet]
        public IActionResult getorder()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // các đơn hàng của shipper chưa hoàn thành
            var order = _context.OrderDetails.Include(x=>x.OrderHeader)
                .Where(x => x.ShipperId == claim.Value && x.Status != "Hoàn thành")
                .Select(x => new
            {
                    headerId = x.OrderHeader.Id,
                    price = x.OrderHeader.Price.GetValueOrDefault().ToString("#,###"),
                    payment = x.OrderHeader.PaymentStatus,
                    address = x.OrderHeader.Address,
                    phone = x.OrderHeader.Phone,
                    status=x.Status,
                    orderDetailId = x.Id
            });
            return Json(new { data = order });
        }
        public IActionResult MyOrders()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ViewBag.ShipperId = claim.Value;
            return View();
        }

        [HttpGet]
        public IActionResult changestatus(string id)
        {
            // cần chuyển qua modal ajax


            var status = _context.OrderDetails.Where(x => x.Id == id).SingleOrDefault();
            //return Json(new { data = status });
            return View(status);
        }
        [HttpPost]
        public IActionResult ChangeStatus(OrderDetail order)
        {
            try
            {
                _context.OrderDetails.Update(order);
                _context.SaveChanges();
            }
            catch
            {

            }
            return RedirectToAction(nameof(MyOrders));
        }
    }
}

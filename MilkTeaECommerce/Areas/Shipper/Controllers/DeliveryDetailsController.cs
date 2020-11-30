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

            var orderHeader = _context.OrderDetails.Include(x=>x.OrderHeader).Include(x=>x.Product)
                .Where(x=>x.Status==null).Select(x => new
            {
                id=x.Id,
                image=x.Product.ImageUrl,
                title=x.Product.Name,
                price=x.Price,
                customer=x.OrderHeader.ApplicationUser.Name,
                address=x.OrderHeader.Address,
                payment=x.OrderHeader.PaymentStatus,
                shopName=x.Product.Shop.Name
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
        public IActionResult Details(string id)
        {
            // xem detail owr index
            if (id == null)
            {
                return NotFound();
            }

            var orderHeader = _context.OrderDetails.Include(x => x.OrderHeader).Include(x => x.Product)
                .Where(x => x.Status == null && x.Id==id).Select(x => new
                {
                    id = x.Id,
                    image = x.Product.ImageUrl,
                    title = x.Product.Name,
                    count=x.Count,
                    price = x.Price,
                    customer = x.OrderHeader.ApplicationUser.Name,
                    address = x.OrderHeader.Address,
                    payment = x.OrderHeader.PaymentStatus,
                    shopName = x.Product.Shop.Name
                }).SingleOrDefault();
            
            if (orderHeader == null)
            {
                return NotFound();
            }

            return Json(new { data = orderHeader });
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
                    id = x.Id,
                    image = x.Product.ImageUrl,
                    title = x.Product.Name,
                    price = x.Price,
                    customer = x.OrderHeader.ApplicationUser.Name,
                    address = x.OrderHeader.Address,
                    payment = x.OrderHeader.PaymentStatus,
                    shopName = x.Product.Shop.Name
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

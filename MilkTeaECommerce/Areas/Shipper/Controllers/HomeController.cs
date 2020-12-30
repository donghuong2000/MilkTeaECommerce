using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Areas.Shipper.Controllers
{
    [Area("Shipper")]
    [Authorize(Roles = "Shipper")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll(string status)
        {
            // var product = _context.OrderDetails.Include(x => x.Product).ToList();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orderHeader = _context.OrderDetails.Include(x => x.OrderHeader).Include(x => x.Product)
                .Where(x => x.Status == status && x.ShipperId == claim.Value).Select(x => new
                {
                    id = x.Id,
                    image = x.Product.ImageUrl,
                    title = x.Product.Name,
                    price = x.Price,
                    customer = x.OrderHeader.ApplicationUser.Name,
                    address = x.OrderHeader.Address,
                    payment = x.OrderHeader.PaymentStatus,
                    shopName = x.Product.Shop.Name,
                    shopAddress = x.Product.Shop.ApplicationUser.Address
                }).ToList();
            return Json(new { data = orderHeader });
        }
        [HttpGet]
        public IActionResult GetAllConfirmed()
        {
            var orderHeader = _context.OrderDetails.Include(x => x.OrderHeader).Include(x => x.Product)
                .Where(x => x.Status == "confirmed").Select(x => new
                {
                    id = x.Id,
                    image = x.Product.ImageUrl,
                    title = x.Product.Name,
                    price = x.Price,
                    customer = x.OrderHeader.ApplicationUser.Name,
                    address = x.OrderHeader.Address,
                    payment = x.OrderHeader.PaymentStatus,
                    shopName = x.Product.Shop.Name,
                    shopAddress = x.Product.Shop.ApplicationUser.Address
                }).ToList();
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

            //var status = _context.OrderDetails.Where(x => x.Id == id).Select(x => new
            //{
            //    status = x.Status
            //});


            var orderHeader = _context.OrderDetails.Include(x => x.OrderHeader).Include(x => x.Product)
               .Where(x => x.Id == id).Select(x => new
               {
                   id = x.Id,
                   image = x.Product.ImageUrl,
                   title = x.Product.Name,
                   count = x.Count,
                   status = x.Status,
                   price = (x.Product.Price * x.Count).GetValueOrDefault().ToString("#,###"),
                   customer = x.OrderHeader.ApplicationUser.Name,
                   address = x.OrderHeader.Address,
                   payment = x.OrderHeader.PaymentStatus,
                   shopName = x.Product.Shop.Name,
                   shopAddress = x.Product.Shop.ApplicationUser.Address,
               }).SingleOrDefault();

            if (orderHeader == null)
            {
                return NotFound();
            }

            return Json(orderHeader);
        }

        [HttpPost]

        public IActionResult Get(string id)
        {
            // add deliveryId cho DeliveryDetail
            var orderDetail = _context.OrderDetails.Where(x => x.Id == id).SingleOrDefault();
            if (orderDetail.Status == "confirmed")      //xác nhận (hiển thị cho shipper ở phần nhận đơn)
            {
                orderDetail.Status = "received";   
            }
            else if (orderDetail.Status == "received")   // hiển thị cho shipper ở phần đã nhận đơn
            {
                orderDetail.Status = "delivery";
            }
            else if(orderDetail.Status=="delivery") // đang vận chuyển ( hiển thị cho shipper ở phần đã lấy hàng)
            {
                orderDetail.Status = "deliveried";
            }

            // add shipper id    
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            orderDetail.ShipperId = claim.Value;

            try
            {
                _context.OrderDetails.Update(orderDetail);
                _context.SaveChanges();
            }
            catch (Exception)
            {

            }
            return Json(new { success = true });
        }

        public IActionResult Cancel(string id)
        {
            var orderDetail = _context.OrderDetails.Where(x => x.Id == id).SingleOrDefault();

            orderDetail.Status = "cancelled";
            _context.OrderDetails.Update(orderDetail);

            /// xử lí cập nhật lại số lượng sản phẩm của shop
            var product = _context.Products.Where(x => x.Id == orderDetail.ProductId).SingleOrDefault();

            product.Quantity += orderDetail.Count;
            _context.Products.Update(product);

            _context.SaveChanges();

            return Json(new { success = true });
        }
        [HttpGet]
        public IActionResult getorder()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            // các đơn hàng của shipper chưa hoàn thành
            var order = _context.OrderDetails.Include(x => x.OrderHeader)
                .Where(x => x.ShipperId == claim.Value && x.Status != "deliveried")
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

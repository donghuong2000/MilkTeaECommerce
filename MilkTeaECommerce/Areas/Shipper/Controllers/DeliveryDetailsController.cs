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
        //public IActionResult GetSelect()
        //{
        //    var select = _context.Deliveries.Select(x => new
        //    {
        //        Id = x.Id,
        //        Text = x.Name
        //    });
        //    return Json(new { items = select });
        //}
        // GET: Shipper/DeliveryDetails
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

        // GET: Shipper/DeliveryDetails/Delete/5
        public async Task<IActionResult> Delete(string id)
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

        // POST: Shipper/DeliveryDetails/Delete/5
        [HttpPost]
        
        public async Task<IActionResult> Get(string id)
        {
            // add deliveryId cho DeliveryDetail
            var orderDetail = _context.OrderDetails.Where(x => x.Id == id).SingleOrDefault();
            orderDetail.Status = "Đã nhận đơn";

            // add shipper id   + add vào shopping cartx    
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //orderDetail.ShipperId = claim.Value;
            var shoppingCart= new ShoppingCart{
                Id=Guid.NewGuid().ToString(),
                ApplicationUserId=claim.Value,
                // id don hang detail
                //ProductId= orderDetail.Id,
                //Count=1
                
            };
            try
            {
                _context.ShoppingCarts.Add(shoppingCart);
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
    }
}

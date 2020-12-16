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
using MilkTeaECommerce.Models.Models;
using Newtonsoft.Json.Linq;
using MilkTeaECommerce.Utility;

namespace MilkTeaECommerce.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CheckoutController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            ViewBag.Deliverymethod = new SelectList(_db.Deliveries.ToList(), "Id", "Name");
            return View();
        }
        public IActionResult CheckDiscount ( string discount)
        {
            //5,121,12312
            
            var dis = _db.Discounts
                .Include(x=>x.DeliveryDiscount).ThenInclude(x=>x.Delivery)
                .Include(x=>x.CategoryDiscount).ThenInclude(x=>x.Category)
                .Include(x=>x.ProductDiscount).ThenInclude(x=>x.Product)
                .FirstOrDefault(x => x.Code == discount);
            if(dis ==null)
            {
                return Json(new { success = false, message = "mã giảm giá ko tồn tại" });

            }
            var obj = new
            {
                id = dis.Id,
                name = dis.Name,
                pt = dis.PercentDiscount,
                category = ListCategory(dis.CategoryDiscount),
                product = ListProduct(dis.ProductDiscount),
                delivery = ListDelivery(dis.DeliveryDiscount),
                de = dis.Description,
                max = dis.MaxDiscount,
                time = dis.TimesUseLimit - dis.TimesUsed,
                a = dis.DateExpired.GetValueOrDefault().ToShortDateString()
            };
            return Json(new { success=true, obj });
        }

        private static string ListCategory(ICollection<CategoryDiscount> categories)
        {
            string name = String.Empty;
            if (categories.Count <1)
                return "";
            foreach (var item in categories)
            {
                name += "," + item.Category.Name;
            }
            return name;
        }
        private static string ListProduct(ICollection<ProductDiscount> products)
        {
            string name = String.Empty;
            if (products.Count < 1)
                return "";
            foreach (var item in products)
            {
                name += "," + item.Product.Name;
            }
            return name;
        }
        private static string ListDelivery(ICollection<DeliveryDiscount> Delivery)
            {
                string name = String.Empty;
                if (Delivery.Count<1)
                    return "";
                foreach (var item in Delivery)
                {
                    name += "," + item.Delivery.Name;
                }
            return name;
            }


        public IActionResult Summary()
        {

            var a = HttpContext.Session.Get<OrderHeader>("cart");
            return View(a);
        }
        [HttpPost]
        public IActionResult Summary(string items, string delivery, string payment, string discountCode)
        {
            //lấy id của thằng đang đăng nhập

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            
            double? discountAmount =0;
            float? DeliveryPrice = 30000;
            List<CartItemViewModel> cartItems = new List<CartItemViewModel>();
            if (items.Length > 0)
            {
                // chuyển về model class rồi từ đây sử lí
                var obj = JArray.Parse(items);
                cartItems = obj.Select(x => x.ToObject<CartItemViewModel>()).ToList();
            }
            var listProduct = cartItems.Select(x => x.add).ToList();
            var listCategory = cartItems.Select(x => _db.Products.Find(x.add).CategoryId).ToList();
            float total = cartItems.Select(x => (x.amount/2)*x.quantity).Sum();
            if (discountCode!=null)
            {
                // tính tiền giảm giá
                var dis = _db.Discounts.Include(x=>x.ProductDiscount)
                    .Include(x=>x.CategoryDiscount)
                    .Include(x=>x.DeliveryDiscount)
                    .FirstOrDefault(x => x.Code == discountCode);
                if (dis != null)
                {
                    // check là giảm giá gì
                    //var isDeliveryDiscount = _db.DeliveryDiscount.Any(x => x.DeliveryId == delivery && x.DiscountId ==dis.Id);
                    //var isCategoryDiscount = _db.CategoryDiscount.Any(x => listCategory.Contains(x.CategoryId) && x.DiscountId == dis.Id);
                    //var isProductDiscount = _db.ProductDiscount.Any(x => listProduct.Contains(x.ProductId) && x.DiscountId == dis.Id);

                    var isDeliveryDiscount = dis.DeliveryDiscount.Any(x => x.DeliveryId == delivery);
                    var isCategoryDiscount = dis.CategoryDiscount.Any(x => listCategory.Contains(x.CategoryId));
                    var isProductDiscount = dis.ProductDiscount.Any(x => listProduct.Contains(x.ProductId));
                    // tính tiền giảm

                    discountAmount = (total * dis.PercentDiscount / 100) > dis.MaxDiscount ? dis.MaxDiscount : (total * dis.PercentDiscount / 100);
                    if (isDeliveryDiscount)
                    {
                        discountAmount = (DeliveryPrice * dis.PercentDiscount / 100) > dis.MaxDiscount ? dis.MaxDiscount : (DeliveryPrice * dis.PercentDiscount / 100);
                    }    
                    if(isProductDiscount)
                    {
                        // lấy tổng tiền của sản phẩm có thể giảm giá
                        var toltalProduct = cartItems.Where(x=> listProduct.Contains(x.add)).Select(x => (x.amount / 2) * x.quantity).Sum();
                        discountAmount = (toltalProduct * dis.PercentDiscount / 100) > dis.MaxDiscount ? dis.MaxDiscount : (toltalProduct * dis.PercentDiscount / 100);
                    }    
                    if(isCategoryDiscount)
                    {
                        var toltalProduct = cartItems
                            .Where(x => listCategory.Contains(_db.Products.Find(x.add).CategoryId))
                            .Select(x => (x.amount / 2) * x.quantity).Sum();
                        discountAmount = (toltalProduct * dis.PercentDiscount / 100) > dis.MaxDiscount ? dis.MaxDiscount : (toltalProduct * dis.PercentDiscount / 100);
                    }
                    if (isCategoryDiscount && isProductDiscount)
                    {
                        var toltalProduct = cartItems
                            .Where(x => listCategory.Contains(_db.Products.Find(x.add).CategoryId) || listProduct.Contains(x.add))
                            .Select(x => (x.amount / 2) * x.quantity).Sum();
                        discountAmount = (toltalProduct * dis.PercentDiscount / 100) > dis.MaxDiscount ? dis.MaxDiscount : (toltalProduct * dis.PercentDiscount / 100);
                    }

                }    
            }
            var headerId = Guid.NewGuid().ToString();
            var curruser = _db.AspNetUsers.Find(claim.Value);
            var header = new OrderHeader 
            { 
                PaymentStatus=discountAmount.ToString(),
                ApplicationUser = curruser ,
                Id = headerId, 
                ApplicationUserId = claim.Value,
                Address = curruser.Address,
                Phone = curruser.PhoneNumber,
                Price = total -  float.Parse(discountAmount.GetValueOrDefault().ToString())
            };

            var deliverydetail = new DeliveryDetail() { Address = header.Address, Price = DeliveryPrice, DateEnd = DateTime.Now.AddDays(10), DateStart = DateTime.Now, DeliveryId = delivery,Delivery=_db.Deliveries.AsNoTracking().FirstOrDefault(x=>x.Id==delivery), Note = ""};
            var ShoppingItem = cartItems.Select(x => new OrderDetail
            {
                Count = x.quantity,
                OrderHeaderId = headerId,
                ProductId = x.add,
                Product = _db.Products.AsNoTracking().FirstOrDefault(a => a.Id == x.add),
                Price = x.amount / 2 * x.quantity,
                Id = Guid.NewGuid().ToString(),
                DeliveryDetails = deliverydetail,
                Status = OrderDetailStatus.unconfirm.ToString()


            }) ;
            header.OrderDetails = ShoppingItem.ToList();

            HttpContext.Session.Set("cart", header);

            return Json(new { url = "/Checkout/summary" });
        }
         
        public IActionResult CancelBill()
        {

            HttpContext.Session.Remove("cart");
            return RedirectToAction("index");
        }

        public IActionResult GenarateBill()
        {
            
            try
            {
                var a = HttpContext.Session.Get<OrderHeader>("cart");
                
                a.ApplicationUser = null;

                var newlistOrderDetail = a.OrderDetails.Select(x => new OrderDetail
                {
                    Count = x.Count,
                    OrderHeaderId = x.OrderHeaderId,
                    ProductId = x.ProductId,
                    Price = x.Price,
                    Id = x.Id,
                    Status = x.Status,
                    DeliveryDetails = new DeliveryDetail() { Address = x.DeliveryDetails.Address, DateEnd = x.DeliveryDetails.DateEnd, Price = x.DeliveryDetails.Price, DateStart = x.DeliveryDetails.DateStart, DeliveryId = x.DeliveryDetails.DeliveryId, Note = x.DeliveryDetails.Note, OrderDetailId = x.DeliveryDetails.OrderDetailId }
                });
                a.OrderDetails = newlistOrderDetail.ToList();

                _db.OrderHeaders.Add(a);
                _db.SaveChanges();
                HttpContext.Session.Remove("cart");
                return Json(new { success = true, message = "Đã tạo đơn hàng thành công" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message });

            }
           



            
        }
    }
}

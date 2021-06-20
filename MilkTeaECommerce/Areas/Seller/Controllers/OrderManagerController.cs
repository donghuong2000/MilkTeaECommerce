using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Seller.Controllers
{
    [Area("seller")]
    [Authorize(Roles = "Manager")]
    public class OrderManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        public OrderManagerController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll(string status,string status1)
        {

            // get current user
            var userid = _userManager.GetUserAsync(User).Result.Id;

            var obj = await _db.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.OrderHeader).ThenInclude(x => x.ApplicationUser)
                .Include(x => x.DeliveryDetails).ThenInclude(x => x.Delivery)
                // order status
                .Where(x => (x.Status == status && x.Product.ShopId == userid) || (x.Status == status1 && x.Product.ShopId == userid)).Select(x => new
                {
                    id = x.Id,
                    image = x.Product.ImageUrl,
                    title = x.Product.Name,
                    num = x.Count,
                    customer = x.OrderHeader.ApplicationUser.UserName,
                    price = x.Product.Price,
                    deliveryInfo = x.DeliveryDetails.Delivery.Name + " : " + x.DeliveryDetails.Price,
                    total = x.Price,
                    status = x.Status
                }).ToListAsync();
            /*hiển thị 
             - hình ảnh sản phảm
             - tiêu đề sản phẩm
             - số lượng sản phẩm
             - người mua
             - đơn giá
             - đơn vị vận chuyển + giá
             - tổng giá
             - nút xác nhận, hủy

            */
            return Json(new { data = obj });
        }

        [HttpPost]
        public IActionResult ChangeStatus(string id, string status)
        {
            var od = _db.OrderDetails.Find(id);
            if (od.Status == OrderDetailStatus.cancelled.ToString())
                return Json(new { success = false , message = "không thể đổi trạng thái vì đơn hàng đã bị hủy"});
            else
            od.Status = status;
            try
            {
                _db.OrderDetails.Update(od);
                _db.SaveChanges();
                return Json(new { success = true, message = "Đã đổi trạng thái thành công" });
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.InnerException.Message });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Controllers
{
    public class RatingController : Controller
    {
        public readonly ApplicationDbContext _db;
        public readonly UserManager<ApplicationUser> _userManager;
        public RatingController (ApplicationDbContext db,UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(string orderdetailid,string productid, string userid, string content, float? rate)
        {
            try
            {

                Rating rating = new Rating() { Id = orderdetailid, ProductId = productid, ApplicationUserId = userid, Content = content, Rate = rate };
                _db.Ratings.Add(rating);
                _db.SaveChanges();
                var orderdetail = _db.OrderDetails.FirstOrDefault(x => x.Id == orderdetailid);
                orderdetail.Status = OrderDetailStatus.evaluated.ToString();
                _db.Update(orderdetail);
                _db.SaveChanges();
                return Json(new { success = true, message = "Thêm thành công đánh giá cho sản phẩm" });
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        // id là product id
        public async Task<IActionResult> Get(string orderdetailid , string productid)
        {
            var user = await _userManager.GetUserAsync(User);
            var product = _db.Products.Include(x=>x.Ratings).FirstOrDefault(x => x.Id == productid);
            var rating = _db.Ratings.FirstOrDefault(x => x.ProductId == product.Id && x.ApplicationUserId == user.Id && x.Id == orderdetailid);
            var status = _db.OrderDetails.FirstOrDefault(x => x.Id == orderdetailid).Status;
            var obj = new
            {
                productid = product.Id,
                productimage = product.ImageUrl,
                user = user.Id,
                productname = product.Name,
                productcategory = _db.Categories.FirstOrDefault(x => x.Id == product.CategoryId).Name,
                rating = rating == null ? null : rating.Rate,
                content = rating == null ? "" : rating.Content,
                status = status ,
                orderdetailid = orderdetailid,
            };

            return Json(new { data = obj });
        }
        public IActionResult Test()
        {
            return Content(OrderDetailStatus.unconfirm.ToString());
        }
    }
}

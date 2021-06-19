using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHostEnvironment _hostEnvironment;
        public UserController(ApplicationDbContext db,SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender, IHostEnvironment hostEnvironment)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Profile");
        }
        public async Task<IActionResult>Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var profile = new ProfileViewModel();
            profile.Email = user.Email;
            profile.Username = user.UserName;
            profile.Name = user.Name;
            profile.Phonenumber = user.PhoneNumber;
            profile.ImageUrl = user.ImageUrl;
            profile.Address = user.Address;
            profile.IsMailComfirm = user.EmailConfirmed;
            profile.IsPhoneComfirm = user.PhoneNumberConfirmed;
            return View(profile);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel vm)
        {
            if(ModelState.IsValid)
            {
                string fileName = "";
                string folderName = "wwwroot\\Media\\";
                string webRootPath = _hostEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (vm.file != null)
                {
                    fileName = ContentDispositionHeaderValue.Parse(vm.file.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        vm.file.CopyTo(stream);

                    }
                }
                var user = await _userManager.FindByNameAsync(vm.Username);
                user.Name = vm.Name;
                user.Address = vm.Address;
                if(vm.Email.ToLower() != user.Email.ToLower())
                {
                    user.UserName = vm.Email;
                    user.Email = vm.Email;
                    user.EmailConfirmed = false;
                }  
                if(vm.Phonenumber!=user.PhoneNumber)
                {
                    user.PhoneNumber = vm.Phonenumber;
                    user.PhoneNumberConfirmed = false;
                }
                if(fileName!="")
                {
                    user.ImageUrl =@"/Media/"+ fileName;
                    vm.ImageUrl = user.ImageUrl;
                }    
                try
                {
                    _db.AspNetUsers.Update(user);
                    _db.SaveChanges();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                    
                }
            }
            return View(vm);
        }
        public async Task<IActionResult> OrderDetail()
        {
            var user = await _userManager.GetUserAsync(User);
            var obj = _db.OrderDetails.Include(x => x.Product).ThenInclude(x => x.Shop)
                .Include(x => x.DeliveryDetails).ThenInclude(x => x.Delivery).Where(x=>x.OrderHeader.ApplicationUserId == user.Id)
                .Select(x => new OrderDetailUserViewModel()
                {
                    shopimage = x.Product.Shop.ImgUrl,
                    shopid = x.Product.Shop.ApplicationUserId,
                    shopname = x.Product.Shop.Name,
                    productimage = x.Product.ImageUrl,
                    productid = x.Product.Id,
                    productname = x.Product.Name,
                    categoryname = _db.Categories.FirstOrDefault(category=>category.Id == x.Product.CategoryId).Name,
                    count = x.Count,
                    deliveryandprice = x.DeliveryDetails.Delivery.Name + ":" + x.DeliveryDetails.Price,
                    status = x.Status,
                    beforeprice = x.Product.Price,
                    afterprice = x.Product.Price,
                    totalprice = x.Price,
                    orderdetailid = x.Id,
                }).ToList();
                
            
            return View(obj);
        }

        //public async IActionResult Rate(string productid, string Content , float? Rate)
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //}
        public IActionResult CancelOrder(string id)
        {
            try
            {
                var o = _db.OrderDetails.Find(id);
                if(o.Status==OrderDetailStatus.unconfirm.ToString())
                {
                    o.Status = OrderDetailStatus.cancelled.ToString();
                    _db.Update(o);
                    _db.SaveChanges();
                    return Json(new { success = true, message = "đã hủy đơn hàng thành công" });
                }
                else
                    return Json(new { success = false, message = "không thể hủy được đơn hàng nữa,vì đơn hàng vừa được xác nhận bởi người bán" });

            }
            catch (Exception e)
            {

                return Json(new { success = true, message = e.Message });
            }
        }
        
        public async Task<IActionResult> ShopChannel()
        {
            var curuser = await _userManager.GetUserAsync(User);
            //check shop exits
            var shop = _db.Shops.FirstOrDefault(x => x.ApplicationUserId == curuser.Id);
            //shop chưa tạo
           
            if (shop == null)
            {
                var CanCreateShop = await _userManager.IsEmailConfirmedAsync(curuser);
                if (CanCreateShop)
                {
                    var newshop = new Shop() { ApplicationUserId = curuser.Id, Name = curuser.Name, ImgUrl = "https://picsum.photos/200/300",Description="",IsConfirm=false };
                    _db.Shops.Add(newshop);
                    _db.SaveChanges();
                    return Json(new { status = true, message = "Wait For Admin Confirm Your Shop" });
                }
                else
                    return Json(new { status = false, message = "Please Confirm your mail to create shop" });
            }
            else
            {
                if (shop.IsConfirm)
                {
                    return Json(new { status = true, message = "OK", url = "/Seller" });
                }
                return Json(new { status = false, message = "Your Shop is pending or has been lock" });
            }    
        }
        public async Task<IActionResult> ShipChannel()
        {
            var curuser = await _userManager.GetUserAsync(User);
           
            
            if (curuser.ShipperRequest == ShipperRequest.Block)
            {
                return Json(new { status = false, message = "Bạn đã bị block" });
            }
            if (curuser.ShipperRequest == ShipperRequest.Pending)
            {
                return Json(new { status = true, message = "Đợi admin duyệt" });
            }
            if (curuser.ShipperRequest == ShipperRequest.Approved)
            {
                return Json(new { status = true, message = "Bạn đã là shipper" });
            }
            var CanCreateShop = await _userManager.IsEmailConfirmedAsync(curuser);
            if (CanCreateShop)
            {
                curuser.ShipperRequest = ShipperRequest.Pending;
                await _userManager.UpdateAsync(curuser);
                return Json(new { status = true, message = "Đợi admin xác nhận" });
            }
            else
                return Json(new { status = false, message = "Xác nhận mail để làm shipper" });

            
        }
    }
}

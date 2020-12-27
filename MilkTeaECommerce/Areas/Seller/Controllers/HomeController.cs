using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Areas.Seller.Controllers
{
    [Area("seller")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public HomeController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShopDetail()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            
            var shop = _db.Shops.Find(claim.Value);
            if (shop != null)
            {
                var vm = new ShopProfileViewModel();
                vm.ApplicationUserId = shop.ApplicationUserId;
                vm.Description = shop.Description;
                vm.Name = shop.Name;
                vm.ImgUrl = shop.ImgUrl;

                return View(vm);
            }
            return NotFound();
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShopDetail(ShopProfileViewModel vm)
        {
            if (ModelState.IsValid)
            {

                string fileName = "";
                string folderName = "wwwroot\\Media\\";
                string webRootPath = _hostEnvironment.ContentRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                }
                if (vm.File != null)
                {
                    fileName = ContentDispositionHeaderValue.Parse(vm.File.ContentDisposition).FileName.Trim('"');
                    string fullPath = Path.Combine(newPath, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        vm.File.CopyTo(stream);
                        vm.ImgUrl = @"/Media/" + fileName;
                    }
                }

                var shop = _db.Shops.Find(vm.ApplicationUserId);
                shop.Name = vm.Name;
                shop.Description = vm.Description;
                if(fileName!="")
                {
                    shop.ImgUrl = vm.ImgUrl;
                }
                else
                {
                    vm.ImgUrl = shop.ImgUrl;
                }    
                try
                {
                    _db.Shops.Update(shop);
                    _db.SaveChanges();
                }
                catch (Exception e)
                {

                    ModelState.AddModelError("", e.InnerException.Message);
                }
            }
            return View(vm);



        }

        // tổng số
        [HttpGet]
        public IActionResult TotalProducts()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var sellerId = claim.Value;

            var total = _db.Products.Where(x => x.ShopId == sellerId).ToList().Count;

            return Json(total);

        }
        public IActionResult Earnings()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var sellerId = claim.Value;


            var totalProduct = _db.OrderDetails.Include(x => x.Product)
                .Where(x => x.Product.ShopId == sellerId && x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).GetValueOrDefault().ToString("#,###");

            return Json(totalProduct);
        }
        public IActionResult TotalCustomer()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var sellerId = claim.Value;

            var customers = _db.OrderDetails.Include(x => x.Product)
                .Where(x => x.Product.ShopId == sellerId && x.Status == OrderDetailStatus.deliveried.ToString())
                .Select(x => x.OrderHeader.ApplicationUserId).Distinct().ToList();

            return Json(customers.Count);
        }
        public IActionResult ChartEarning()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var sellerId = claim.Value;

            // lấy các sản orderdetail của shop có ngày giao rồi

            var data=_db.DeliveryDetails.Include(x=>x.OrderDetail).ThenInclude(x=>x.Product).ThenInclude(x=>x.Shop)
                .Where(x=>x.OrderDetail.Product.ShopId==sellerId &&
                x.OrderDetail.Status== OrderDetailStatus.deliveried.ToString())
                .OrderByDescending(x=>x.DateEnd)
                .Select(x => new
                {
                    price = x.OrderDetail.Price,
                    date = x.DateEnd.GetValueOrDefault()
                }).ToList();
            int[] month = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            var lst = new List<object>();
            foreach (var item in month)
            {
                var x = data.Where(x => x.date.Month == item && x.date.Year==DateTime.Now.Year).Sum(x => x.price).Value;
                lst.Add(x);
            }
            

            return Json(lst);
        }
    }
}

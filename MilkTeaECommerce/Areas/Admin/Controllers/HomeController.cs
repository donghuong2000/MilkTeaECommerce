using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Utility;
namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostEnvironment _hostEnvironment;
        public HomeController(ApplicationDbContext db, IHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;

        }
        public IActionResult Index()
        {


            ViewBag.ListShop = new SelectList(_db.Shops.ToList(), "ApplicationUserId", "Name");



            ViewBag.ProductCount = _db.Products.Count();
            ViewBag.UserCount = _db.AspNetUsers.Count();
            ViewBag.Allowen = _db.OrderDetails.Where(x => x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).GetValueOrDefault().ToString("#,###")+ " VND";
            return View();
        }
        /// <summary>
        /// thống kê doanh thu tất cả hoặc 1 cá nhân theo năm nào đó
        /// đầu vào là id của user và năm cần thống kê
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <returns> 
        /// trả về chỗi json gồm labels và các giá trị tương ứng vs từng label đó
        /// </returns>
        public IActionResult Statistical_Revenue(string id, DateTime? date)
        {
            var labels = new string[12];
            var values = new float[12];
            if (id==null || date == null)
            {
                var obj = _db.OrderDetails
                    .Include(x => x.Product).ThenInclude(x => x.Shop)
                    .Where(x=>x.Status == OrderDetailStatus.deliveried.ToString())
                    .GroupBy(x => x.Product.Shop.Name)
                    
                    .Select(statis => new
                    {
                        key = statis.Key,
                        sum = _db.OrderDetails.Include(x => x.Product)
                        .ThenInclude(x => x.Shop)
                        .Where(x => x.Product.Shop.Name == statis.Key && x.Status == OrderDetailStatus.deliveried.ToString())
                        .Sum(x => x.Price)
                    }).ToList();
                labels = obj.Select(x => x.key).ToArray();
                values = obj.Select(x => x.sum.GetValueOrDefault()).ToArray();
                return Json(new {labels,values });
            }
            var obj1 = _db.OrderDetails
                .Include(x => x.Product)
                .Include(x => x.DeliveryDetails)
                .Where(x => x.Status == OrderDetailStatus.deliveried.ToString() && x.Product.ShopId == id)
                .Select(x => new
                {
                    date = x.DeliveryDetails.DateEnd.GetValueOrDefault(),
                    price = x.Price
                }).ToList();
            var obj2 = obj1
                .GroupBy(x => new { x.date.Year, x.date.Month })
                .Select(x => new
                {
                    month = x.Key.Month,
                    year = x.Key.Year,
                    sum = obj1.Where(a => a.date.Year == x.Key.Year && a.date.Month == x.Key.Month).Sum(x => x.price)
                }).ToList();

            for (int i = 0; i < 12; i++)
            {
                labels[i] ="Tháng "+ (i + 1).ToString();
                var s = obj2.Where(x => x.month == (i + 1) && x.year == date.GetValueOrDefault().Year).FirstOrDefault();
                values[i] = s == null ? 0 : s.sum.GetValueOrDefault();
                    
            }

            return Json(new { labels, values });
        }

        public IActionResult Statistical_Product(DateTime? date)
        {
            if(date == null)

            {
                var obj = _db.OrderDetails.Include(x => x.Product)
                    .Where(x => x.Status == OrderDetailStatus.deliveried.ToString())
                    .GroupBy(x => x.Product.Name)
                    .Select(a => new
                    {
                        name = a.Key,
                        count = a.Sum(x=>x.Count)
                    }).OrderByDescending(x=>x.count).ToList();

                var labels = obj.Select(x => x.name).ToArray();
                var values = obj.Select(x => x.count).ToArray();
                return Json(new {labels,values });
            }
            else
            {
                var obj = _db.OrderDetails.Include(x => x.Product)
                    .Include(x => x.DeliveryDetails)
                    .Where(x => x.Status == OrderDetailStatus.deliveried.ToString()).ToList();
                var abc = obj.Where(x=>x.DeliveryDetails.DateEnd.GetValueOrDefault().Year==date.GetValueOrDefault().Year &&
                x.DeliveryDetails.DateEnd.GetValueOrDefault().Month == date.GetValueOrDefault().Month
                )
                    .GroupBy(x => x.Product.Name)
                    .Select(a => new
                    {
                        name = a.Key,
                        count = a.Sum(x => x.Count)
                    }).OrderByDescending(x => x.count).ToList();

                var labels = abc.Select(x => x.name).ToArray();
                var values = abc.Select(x => x.count).ToArray();
                return Json(new { labels, values });
            }
            
            
        }



        [HttpPost]
        public IActionResult ImportExcel(IFormFile files)
        {
            
            if (files == null)
            {
                return Json(new { success = false, message = "Ko đọc được file" });
            }
               
            List<Product> products = new List<Product>();
            string fileName = "";
            string folderName = "wwwroot\\Media\\";
            string webRootPath = _hostEnvironment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string fullPath = "";
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (files != null )
            {
                fileName = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName.Trim('"');
                fullPath = Path.Combine(newPath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    files.CopyTo(stream);
                }
            }
            try
            {
                var dataset = ExcelProvider.Read(fullPath);
                var ExcelTable = dataset.Tables[0];
                foreach (DataRow row in ExcelTable.Rows)
                {

                    var a = row.ItemArray[0].ToString();

                    if (a != "Id")
                    {
                        var p = new Product
                        {
                            Id = row.ItemArray[0].ToString(),
                            Name = row.ItemArray[1].ToString(),
                            Description = row.ItemArray[2].ToString(),
                            ImageUrl = row.ItemArray[3].ToString(),
                            Price = float.Parse(row.ItemArray[4].ToString()),
                            IsConfirm = bool.Parse(row.ItemArray[5].ToString()),
                            Quantity = int.Parse(row.ItemArray[6].ToString()),
                            CategoryId = row.ItemArray[7].ToString(),
                            ShopId = row.ItemArray[8].ToString()

                        };
                        products.Add(p);
                    }
                }
                _db.Products.AddRange(products);
                _db.SaveChanges();
                return Json(new { success = true, message = "Thêm danh sách sản phẩm bằng excel thành công" });
            }
            catch (Exception e)
            {

                return Json(new { success = false, message = e.InnerException.Message });
            }
            
        }
    }
}

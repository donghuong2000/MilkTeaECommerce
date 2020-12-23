using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
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
            return View();
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

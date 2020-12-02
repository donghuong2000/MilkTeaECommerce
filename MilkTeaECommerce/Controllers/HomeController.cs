using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext db,ILogger<HomeController> logger)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(int? page)
        {
            
            int num = 9;
            var listProduct = _db.Products
                .Where(x=>x.IsConfirm == true)
                // không chuyển đổi ở bước này vì nó sẽ tốn nất nhiều tài nghuyên để chuyển đổi 1 số lượng lớn object
                //.Select(x => new ProductViewModel
                //{
                //    ProductId = x.Id,
                //    ProductName = x.Name,
                //    ProductImgUrl = x.ImageUrl,
                //    Price = x.Price.GetValueOrDefault(),
                //    OldPrice = x.Price.GetValueOrDefault()*2
                //})
                ;
            int ProductCount = listProduct.Count();
            // điều kiện để phân trang
            if (page!=null && page >0)
            {

                /*số trang có thể có:
                 *TH1: ví dụ có 18 sản phẩm, mỗi trang chứa 9 sản phẩm thì có TỐI ĐA 2 TRANG
                 *TH2: ví dụ có 20 sản phẩm, vậy có tối đa 3 trang và trang cuối chứa 2 sản phẩm 
               */
                int NumPageCanHave = ProductCount % num == 0 ? ProductCount / num : (ProductCount / num) + 1;

                ViewBag.page = page;
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= NumPageCanHave && ProductCount % num !=0)
                {
                    
                    // lấy số lẻ
                    listProduct = listProduct.Skip(num * (NumPageCanHave - 1)).Take(ProductCount % num);
                       
                }
                //trường hợp 1
                else 
                {
                    
                    listProduct = listProduct.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }    
               
                
                
            }
            else
            {
                ViewBag.page = 1;
                listProduct = listProduct.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = listProduct
                .Select(x => new ProductViewModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name,
                    ProductImgUrl = x.ImageUrl,
                    Price = x.Price.GetValueOrDefault(),
                    OldPrice = x.Price.GetValueOrDefault() * 2
                }).ToList()
                ;
            return View(obj);
            
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}

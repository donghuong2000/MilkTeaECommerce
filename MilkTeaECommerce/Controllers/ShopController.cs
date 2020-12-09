using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ShopController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? page)
        {
            int num = 9;
            var lstShop = _context.Shops.Include(x => x.ApplicationUser).Where(x => x.IsConfirm == true);
                
            int shopCount = lstShop.Count();
            // điều kiện để phân trang
            if (page != null && page > 0)
            {
                int numpage = shopCount % num == 0 ? shopCount / num : (shopCount / num) + 1;

                // trang cuoi +1
                if(page>numpage)
                {
                    return RedirectToAction();
                }
                ViewBag.page = page;
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && shopCount % num != 0)
                {
                    // lấy số lẻ
                    lstShop = lstShop.Skip(num * (numpage - 1)).Take(shopCount % num);

                }
                //trường hợp 1
                else
                {

                    lstShop = lstShop.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }
            }
            else
            {
                ViewBag.page = 1;
                lstShop = lstShop.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = lstShop
                .Select(x => new ShopViewModel
                {
                    Id = x.ApplicationUserId,
                    Name = x.Name,
                    ImgUrl = x.ImgUrl,
                    Address = x.ApplicationUser.Address

                }).ToList();
            return View(obj);
        }
        [HttpGet]
        public IActionResult Details(string id,int? page)
        {
            if (id == null)
            {
                return NotFound();
            }
            
             var shop = _context.Shops.Include(x=>x.ApplicationUser).Where(x => x.ApplicationUserId == id).SingleOrDefault();
            if (shop == null)
            {
                return NotFound();
            }
            ViewBag.IdShop = shop.ApplicationUserId;
            ViewBag.NameShop = shop.Name;
            ViewBag.DeShop = shop.Description;
            ViewBag.Image = shop.ImgUrl;
            ViewBag.Address = shop.ApplicationUser.Address;

            int num = 9;
            var lstProduct = _context.Products.Where(x => x.ShopId==id && x.IsConfirm==true);

            int procount = lstProduct.Count();
            // điều kiện để phân trang
            if (page != null && page > 0)
            {
                int numpage = procount % num == 0 ? procount / num : (procount / num) + 1;

                // trang cuoi +1
                if (page > numpage)
                {
                    ViewBag.page = numpage;
                }
                else
                {
                    ViewBag.page = page;
                }
                // nếu là trang cuối và là rơi vào trường hơp 2
                if (page >= numpage && procount % num != 0)
                {
                    // lấy số lẻ
                    lstProduct = lstProduct.Skip(num * (numpage - 1)).Take(procount % num);

                }
                //trường hợp 1
                else
                {

                    lstProduct = lstProduct.Skip(num * (page.GetValueOrDefault() - 1)).Take(num);
                }
            }
            else
            {
                ViewBag.page = 1;
                lstProduct = lstProduct.Take(9);
            }
            // chuyển product thành ProductViewModel
            var obj = lstProduct
                .Select(x => new ProductViewUserModel
                {
                    ProductId = x.Id,
                    ProductName = x.Name,
                    ProductImgUrl = x.ImageUrl,
                    Price = x.Price.GetValueOrDefault(),
                    OldPrice=x.Price.GetValueOrDefault()*2
                }).ToList();
            return View(obj);
        }

        // api
        [HttpGet]
        public IActionResult getshop(string id)
        {
            var shop = _context.Shops.Include(x => x.ApplicationUser).Where(x => x.ApplicationUserId == id).Select(x => new
            {
                Id=x.ApplicationUserId,
                Name=x.Name,
                Des=x.Description,
                Image=x.ImgUrl,
                Address=x.ApplicationUser.Address
            }).SingleOrDefault();
            return Json(shop);
        }
        [HttpGet]
        public IActionResult getproducts(string idShop)
        {
            var products = _context.Products.Where(x => x.ShopId == idShop && x.IsConfirm== true).Select(x => new
            {
                Id=x.Id,
                Name=x.Name,
                Price=x.Price.GetValueOrDefault().ToString("#,###"),
                Image=x.ImageUrl
            });
            return Json(products);
        }
    }
}

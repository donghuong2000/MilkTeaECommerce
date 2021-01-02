using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private static float average_rating_shop ;
        private static float average_rating_product ;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Detail(string id)
        {
            if (id == null)
            {
                return RedirectToAction("index", "home");
            }
            else
            {
                var product = _db.Products.Include(x => x.Category).Include(x => x.Shop).Include(x=>x.Ratings).ThenInclude(x=>x.ApplicationUser).FirstOrDefault(x => x.Id == id);
                if (product != null)
                {
                    Get_Info_Of_Shop_And_Product(product);
                    var rating_shop = _db.Ratings.Include(x => x.Product).Where( x=>x.Product.ShopId == product.ShopId).ToList();
                    if(rating_shop.Count >0)
                    {
                        var average_rating_shop_calculate = (double)(rating_shop.Average(x => x.Rate));
                        average_rating_shop = Convert.ToSingle(Math.Floor(average_rating_shop_calculate));
                    }
                    else
                        average_rating_shop = 0;

                    var rating_product_of_shop = _db.Ratings.Include(x => x.Product).Where(x => x.ProductId == product.Id && x.Product.ShopId == product.ShopId).ToList();
                    if (rating_product_of_shop.Count > 0)
                    {
                        var average_rating_product_calculate = (double)(rating_product_of_shop.Average(x => x.Rate));
                        average_rating_product = Convert.ToSingle(Math.Floor(average_rating_product_calculate));
                    }
                    else
                        average_rating_product = 0;
                    return View(product);
                }
                return NotFound();
            }
        }

        public IActionResult Get_avarage_rating_shop_and_product()
        {
            // trả về điểm đánh giá của shop và của product
            return Json(new { rating_shop = average_rating_shop , rating_product = average_rating_product });
        }

        public void Get_Info_Of_Shop_And_Product(Product product)
        {
            var product_list_of_shop = _db.Products.Include(x => x.Ratings).Where(x => x.ShopId == product.Shop.ApplicationUserId).ToList();
            // số lượng sản phẩm của shop
            ViewBag.Product_Count = product_list_of_shop.Count();
            // số lượt đánh giá
            ViewBag.Rate_Count = product_list_of_shop.Select(x => x.Ratings.Count()).Sum();
            //  số lượt người đã mua sản phẩm của shop
            var list_customer_buy_product_of_shop = _db.OrderDetails.Include(x => x.Product).Where(x => x.Product.ShopId == product.Shop.ApplicationUserId && x.Status == OrderDetailStatus.deliveried.ToString()).ToList();
            ViewBag.Customer_Bought = list_customer_buy_product_of_shop.Count();
            // Phần trăm đơn hàng giao thành công( là bằng số đơn hàng có status = deliveried chia cho tổng đơn hàng có status = deliveried hoặc cancelled)
            var number_order_detail_of_shop = _db.OrderDetails.Include(x => x.Product).Where(x => x.Product.ShopId == product.Shop.ApplicationUserId && (x.Status == OrderDetailStatus.deliveried.ToString() || x.Status == OrderDetailStatus.cancelled.ToString())).Count();
            double result = 0;
            if(list_customer_buy_product_of_shop.Count == 0 && number_order_detail_of_shop == 0)
            {
                result = 1;
            }
            else
                result = (double)(list_customer_buy_product_of_shop.Count()) / (double)(number_order_detail_of_shop);
            ViewBag.Success_Percent = (result*100).ToString("0.00");

            // Số lượng đã bán của sản phẩm đang xem
            ViewBag.product_selled_count = _db.OrderDetails.Where(x => x.ProductId == product.Id && x.Status == OrderDetailStatus.deliveried.ToString()).Count();
            ViewBag.product_rate_count = _db.Ratings.Where(x => x.ProductId == product.Id).Count();

        }

    }
}

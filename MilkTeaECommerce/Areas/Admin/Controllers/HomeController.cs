using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Utility;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
   //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            //var listshop = _db.Shops.ToList();

            //ViewData["listshop"] = _db.Shops.Select(c => new SelectListItem()
            //{
            //    Text = c.Name,
            //    Value = c.ApplicationUserId
            //}).ToList();
            var lsShop = _db.Shops.Select(c => new SelectListItem()
            {
                Text = c.Name,
                Value = c.ApplicationUserId
            }).ToList();
            var parameter = new DynamicParameters();
            parameter.Add("@shopid",lsShop[0].Value.ToString());
            // lấy về dưới dạng json
            var obj = SP_Call.ExecuteJson("statistic_number", parameter);//ok
            TempData["earning"] = obj.Select(x => x["earning"].ToString()).ToList()[0].ToString();
            TempData["sumproduct"] = obj.Select(x => x["count_product"].ToString()).ToList()[0].ToString();
            TempData["sumcustomer"] = obj.Select(x => x["count_cus"].ToString()).ToList()[0].ToString();
            return View(lsShop);
        }
        //public IActionResult GetDataTable(string Id)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        //{
        //    var parameter = new DynamicParameters();
        //    parameter.Add("@shopid", Id);
        //    var obj = SP_Call.ExecuteJson("USP_stastisticTable", parameter);//ok
        //    var list = obj.ToList();
        //    //var lstCustomerid = obj.Select(x => x["cusid"].ToString()).ToList();
        //    //var lstCustomerMoney = obj.Select(x => x["SumMoney"].ToString()).ToList();
        //    return Json(new { data = list });

        //}
        public IActionResult GetDatanumber(string Id)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {
            string earning;
            string sumproduct;
            string sumcustomer;
            var parameter = new DynamicParameters();
            parameter.Add("@shopid", Id);
            var obj1 = SP_Call.ExecuteJson("statistic_number", parameter);//ok
            if(obj1 == null)
            {
                earning = "0";
                sumproduct = "0";
                sumcustomer = "0";
                return Json(new { earning, sumproduct, sumcustomer });
            }    
            earning = obj1.Select(x => x["earning"].ToString()).ToList()[0].ToString();
            sumproduct = obj1.Select(x => x["count_product"].ToString()).ToList()[0].ToString();
            sumcustomer = obj1.Select(x => x["count_cus"].ToString()).ToList()[0].ToString();
            return Json(new { earning,sumproduct,sumcustomer });
            
        }
        public IActionResult GetData(string Id)//xong qua đây  // id truyền vào null kìa// null nữa gòi
        {
            //var totalShop = _db.Shops.Include(x => x.Products).Where(x => x.ApplicationUserId == Id);

            //var orderCount = _db.OrderDetails
            //    .Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id)
            //    .Select(x => new { 
            //        product = x.Product.Name,
            //        price = x.Price
            //    })
            //    .GroupBy(x=>x.product)
            //    .Select(x=> new {
            //        Key = x.Key,
            //        Value = x.Sum(s => s.price)    
            //    }).ToList();

            //var a = new List<string>();
            //var b = new List<float>();
            //foreach (var item in orderCount)
            //{
            //    SumPrice += (float)item.Value;
            //    a.Add(item.Key);
            //    b.Add((float)item.Value);
            //}

            float SumPrice = 0;
            string[] labels = new string[12];
            labels = new string[]{ "1","2", "3","4","5", "6","7","8","9","10","11", "12" };
            var values = new float[12];
            values = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var parameter = new DynamicParameters();
            parameter.Add("@shopid", Id);
            // lấy về dưới dạng json
            var obj = SP_Call.ExecuteJson("USP_Statistic", parameter);//ok
            List<string> label = new List<string>();
            List<float> value = new List<float>();
            try
            {
                label = obj.Select(x => x["stastistic_month"].ToString()).ToList();
                value = obj.Select(x => float.Parse(x["sum_price"].ToString())).ToList();
            }
            catch
            {
            }
            if(label != null)
            {
                int i = 0;
                foreach(var item in label)
                {
                    values[Convert.ToInt32(item)-1] = value[i];
                    SumPrice += value[i];
                    i++;
                }    
            }
            
            return Json(new { labels,values});
            //var totalProduct = _db.OrderDetails.Include(x => x.Product)
            //    .Where(x => x.Product.ShopId == Id && x.Status == OrderDetailStatus.deliveried.ToString()).Sum(x => x.Price).ToString();
            //return NotFound();
        }
    }
}

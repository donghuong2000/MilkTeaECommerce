using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.DataAccess.Extension;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;
using SQLitePCL;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiscountsController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: Admin/Discounts
        public async Task<IActionResult> Index()
        {
           
            return View(await _context.Discounts.ToListAsync());
        }

        public IActionResult GetAll()
        {
            var obj = _context.Discounts.Select(x => new
            {
                id = x.Id,
                name = x.Name,
                des = x.Description,
                dateStart = x.DateStart.GetValueOrDefault().ToShortDateString(),
                dateEnd=x.DateExpired.GetValueOrDefault().ToShortDateString(),
                timeuselimit=x.TimesUseLimit,
                per=x.PercentDiscount,
                max=x.MaxDiscount,
                code=x.Code
            });

            return Json(new { data = obj });
        }
        [HttpGet]
        public async Task<IActionResult> Upsert(string id)
        {
               
            // lấy giá trị cho các select 
            var category = _context.Categories.ToList();
            ViewBag.Categories = new SelectList(category, "Id", "Name");

            var delivery = _context.Deliveries.ToList();
            ViewBag.Deliveries = new SelectList(delivery, "Id", "Name");

            var product = _context.Products.ToList();
            ViewBag.Products = new SelectList(product, "Id", "Name");

            if (id!=null)
            {
                // Edit
                // Chuyển qua discountviewmodel để hiển thị

                var discount = await _context.Discounts.Include(x => x.CategoryDiscount).Include(x => x.DeliveryDiscount)
                       .Include(x => x.ProductDiscount).FirstOrDefaultAsync(x => x.Id == id);


                if (discount.Id==null)
                {
                    return NotFound();
                }    
                var dv = new DiscountViewModel()
                {
                    Id=discount.Id,
                    Name = discount.Name,
                    Description = discount.Description,
                    DateStart = discount.DateStart,
                    DateExpired = discount.DateExpired,
                    TimesUseLimit = discount.TimesUseLimit,
                    PercentDiscount = discount.PercentDiscount,
                    MaxDiscount = discount.MaxDiscount,
                    Code = discount.Code,
                    CategoryDiscounts = discount.CategoryDiscount.Select(x => x.CategoryId).ToList(),
                    DeliveryDiscounts = discount.DeliveryDiscount.Select(x => x.DeliveryId).ToList(),
                    ProductDiscounts = discount.ProductDiscount.Select(x => x.ProductId).ToList()
                };
                ViewBag.Id = id;


                return View(dv);
            }
            ViewBag.Id = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DiscountViewModel discount)
        {
            if (ModelState.IsValid)
            {
                
                // check code is UNIQUE
                var code = (from c in _context.Discounts
                            where c.Id!= discount.Id && c.Code==discount.Code
                            select c).ToList();

                if (code.Count > 0)
                {
                    ModelState.AddModelError("Code", "Code đã tồn tại");
                    ViewBag.Id = "";
                    
                }
                // check ngay hop le 
                if (discount.DateExpired < discount.DateStart)
                {
                    ModelState.AddModelError("DateExpired", "Ngày không hợp lệ");
                }
                if (ModelState.ErrorCount>0)
                {
                    var category = _context.Categories.ToList();
                    ViewBag.Categories = new SelectList(category, "Id", "Name");

                    var delivery = _context.Deliveries.ToList();
                    ViewBag.Deliveries = new SelectList(delivery, "Id", "Name");

                    var product = _context.Products.ToList();
                    ViewBag.Products = new SelectList(product, "Id", "Name");

                    return View(discount);
                }    
                // Thêm khuyến mãi
                if (discount.Id == null)
                {
                    var d = new Discount()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = discount.Name,
                        Description = discount.Description,
                        DateStart = discount.DateStart,
                        DateExpired = discount.DateExpired,
                        //mặc định timeused khi tạo =0
                        TimesUsed = 0,
                        TimesUseLimit = discount.TimesUseLimit,
                        PercentDiscount = discount.PercentDiscount,
                        MaxDiscount = discount.MaxDiscount,
                        Code = discount.Code
                    };

                    // gán các CategoryDiscount, Delivery....
                    // dungf sel
                    foreach (var item in discount.CategoryDiscounts)
                    {
                        _context.CategoryDiscount.Add(new CategoryDiscount() { CategoryId=item,DiscountId=d.Id });

                    }
                    foreach (var item in discount.DeliveryDiscounts)
                    {
                        _context.DeliveryDiscount.Add(new DeliveryDiscount() { DeliveryId = item, DiscountId = d.Id });
                    }
                    foreach (var item in discount.ProductDiscounts)
                    {
                        _context.ProductDiscount.Add(new ProductDiscount() { ProductId = item, DiscountId = d.Id });
                    }

                    _context.Discounts.Add(d);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var dis = await _context.Discounts.Include(x => x.CategoryDiscount).Include(x => x.DeliveryDiscount)
                        .Include(x => x.ProductDiscount).FirstOrDefaultAsync(x=>x.Id == discount.Id);


                    //// Chỉnh sửa
                    _context.TryUpdateManyToMany(dis.CategoryDiscount, discount.CategoryDiscounts.Select(x => new CategoryDiscount
                    {
                        CategoryId = x,
                        DiscountId = dis.Id
                    }), x => x.CategoryId);

                    _context.TryUpdateManyToMany(dis.ProductDiscount, discount.ProductDiscounts.Select(x => new ProductDiscount
                    {
                        ProductId = x,
                        DiscountId = dis.Id
                    }), x => x.ProductId);

                    _context.TryUpdateManyToMany(dis.DeliveryDiscount, discount.DeliveryDiscounts.Select(x => new DeliveryDiscount
                    {
                        DeliveryId = x,
                        DiscountId = dis.Id
                    }), x => x.DiscountId);

                    // gán giá trị từ discountviewmodel cho discout va update
                    dis.Id = discount.Id;
                    dis.Name = discount.Name;
                    dis.Description = discount.Description;
                    dis.DateStart = discount.DateStart;
                    dis.DateExpired = discount.DateExpired;
                    dis.TimesUseLimit = discount.TimesUseLimit;
                    dis.PercentDiscount = discount.PercentDiscount;
                    dis.MaxDiscount = discount.MaxDiscount;
                    dis.Code = discount.Code;

                    _context.Discounts.Update(dis);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction();
            }

        }
      
        // GET: Admin/Discounts/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts
                .FirstOrDefaultAsync(m => m.Id == id);

            if (discount == null)
            {
                return NotFound();
            }
            var d = await _context.Discounts.Include(x => x.CategoryDiscount).Include(x => x.DeliveryDiscount)
                .Include(x => x.ProductDiscount).FirstOrDefaultAsync(x => x.Id==id);

            _context.CategoryDiscount.RemoveRange(d.CategoryDiscount);
            _context.DeliveryDiscount.RemoveRange(d.DeliveryDiscount);
            _context.ProductDiscount.RemoveRange(d.ProductDiscount);
            _context.Discounts.Remove(d);
            await _context.SaveChangesAsync();
            return Json(new { success=true});
        }


        private bool DiscountExists(string id)
        {
            return _context.Discounts.Any(e => e.Id == id);
        }
    }
}

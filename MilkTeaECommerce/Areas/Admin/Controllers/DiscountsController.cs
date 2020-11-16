using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkTeaECommerce.Data;
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
        public async Task<IActionResult> Upsert(string? id)
        {
               
            // lấy giá trị cho các select 
            var category = _context.Categories.ToList();
            ViewBag.Categories = category;

            var delivery = _context.Deliveries.ToList();
            ViewBag.Deliveries = delivery;

            var product = _context.Products.ToList();
            ViewBag.Products = product;

            if(id!=null)
            {
                // Edit
                // Chuyển qua discountviewmodel để hiển thị
                
                var discount = (from d in _context.Discounts
                                where d.Id == id
                                select d).SingleOrDefault();
                if(discount.Id==null)
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
                    Code = discount.Code
                };
                ViewBag.Id = id;

                // select CategoryDiscount
                var cd = (from c in _context.CategoryDiscount
                          where c.DiscountId == dv.Id
                          select c.CategoryId).ToList();
                ViewBag.CategoryId = cd;
                var dd=(from d in _context.DeliveryDiscount
                       where d.DiscountId==dv.Id
                       select d.DeliveryId).ToList();
                ViewBag.DeliveryId = dd;
                var pd = (from p in _context.ProductDiscount
                          where p.DiscountId == dv.Id
                          select p.ProductId).ToList();
                ViewBag.ProductId = pd;
                return View(dv);
            }
            ViewBag.Id = "";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(DiscountViewModel discount, List<string> selCategory,
                                    List<string> selDelivery, List<string> selProduct)
        {
            if (ModelState.IsValid)
            {

                // check code is UNIQUE
                var code = (from c in _context.Discounts
                            where c.Id!= discount.Id && c.Code==discount.Code
                            select c).ToList();
                if (code.Count > 0)
                {
                    return View("Upsert", discount);
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
                        Code = discount.Code,
                        CategoryDiscount=discount.CategoryDiscounts,
                        DeliveryDiscount=discount.DeliveryDiscounts,
                        ProductDiscount=discount.ProductDiscounts
                    };
                    
                    // gán các CategoryDiscount, Delivery....
                    foreach (var item in selCategory)
                    {
                        d.CategoryDiscount.Add(new CategoryDiscount() { CategoryId = item, DiscountId = d.Id });
                    }
                    foreach (var item in selDelivery)
                    {
                        d.DeliveryDiscount.Add(new DeliveryDiscount() { DeliveryId = item, DiscountId = d.Id });
                    }
                    foreach (var item in selProduct)
                    {
                        d.ProductDiscount.Add(new ProductDiscount() { ProductId = item, DiscountId = d.Id });
                    }

                    _context.Discounts.Add(d);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    // Chỉnh sửa
                    var d = new Discount()
                    {
                        // phân tách id category
                        Id=discount.Id,
                        Name = discount.Name,
                        Description = discount.Description,
                        DateStart = discount.DateStart,
                        DateExpired = discount.DateExpired,
                        //không update TimeUsed

                        TimesUseLimit = discount.TimesUseLimit,
                        PercentDiscount = discount.PercentDiscount,
                        MaxDiscount = discount.MaxDiscount,
                        Code = discount.Code
                    };

                    // remove các  CategoryDiscount, Delivery.... và gán lại
                    var cdel = (from c in _context.CategoryDiscount
                              where c.DiscountId == d.Id
                              select c).ToList();
                    var ddel=(from de in _context.DeliveryDiscount
                             where de.DiscountId==d.Id
                             select de).ToList();
                    var pdel = (from p in _context.ProductDiscount
                                where p.DiscountId == d.Id
                                select p).ToList();
                    foreach (var item in cdel)
                    {
                        _context.CategoryDiscount.Remove(item);
                    }
                    foreach (var item in ddel)
                    {
                        _context.DeliveryDiscount.Remove(item);
                    }
                    foreach (var item in pdel)
                    {
                        _context.ProductDiscount.Remove(item);
                    }
                    foreach (var item in selCategory)
                    {
                        d.CategoryDiscount.Add(new CategoryDiscount() { CategoryId = item, DiscountId = d.Id });

                    }
                    foreach (var item in selDelivery)
                    {
                        d.DeliveryDiscount.Add(new DeliveryDiscount() { DeliveryId = item, DiscountId = d.Id });
                    }
                    foreach (var item in selProduct)
                    {
                        d.ProductDiscount.Add(new ProductDiscount() { ProductId = item, DiscountId = d.Id });
                    }

                    var disc = (from dc in _context.Discounts
                                where dc.Id == discount.Id
                                select dc).SingleOrDefault();
                    _context.Discounts.Remove(disc);
                    _context.Discounts.Add(d);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return RedirectToAction();
            }

        }
        // GET: Admin/Discounts/Details/5
        public async Task<IActionResult> Details(string id)
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

            return View(discount);
        }

        // GET: Admin/Discounts/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discount = await _context.Discounts.FindAsync(id);
            if (discount == null)
            {
                return NotFound();
            }
            return View(discount);
        }

        // POST: Admin/Discounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,DateStart,DateExpired,TimesUsed,TimesUseLimit,PercentDiscount,MaxDiscount,Code")] Discount discount)
        {
            if (id != discount.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountExists(discount.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(discount);
        }

        // GET: Admin/Discounts/Delete/5
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

            return View(discount);
        }

        // POST: Admin/Discounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var discount = await _context.Discounts.FindAsync(id);
            _context.Discounts.Remove(discount);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountExists(string id)
        {
            return _context.Discounts.Any(e => e.Id == id);
        }
    }
}

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

        // GET: Admin/Discounts/Create
        public IActionResult Create()
        {
            // gán giá trị cho combobox cho ViewModel
            var category = _context.Categories.ToList();
            ViewBag.Categories = category;

            var delivery = _context.Deliveries.ToList();
            ViewBag.Deliveries = delivery;

            var product = _context.Products.ToList();
            ViewBag.Products = product;

            return View("CreateViewModel");
        }

        // POST: Admin/Discounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscountViewModel discount)
        {
            // chưa check được số lượng khuyến mãi
            // chưa compare được ngày hợp lệ
            // chưa compare được ngày kết thúc hợp lệ
            if (ModelState.IsValid)
            {
                var d = new Discount()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = discount.Name,
                    Description = discount.Description,
                    DateStart = discount.DateStart,
                    DateExpired = discount.DateExpired,
                    
                    //mặc định timeused khi tạo =0
                    TimesUsed=0,
                    TimesUseLimit = discount.TimesUseLimit,
                    PercentDiscount = discount.PercentDiscount,
                    MaxDiscount = discount.MaxDiscount,
                    Code = discount.Code
                };
                _context.Discounts.Add(d);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

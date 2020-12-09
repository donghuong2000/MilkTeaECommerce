using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models.Models;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Areas.Seller.Controllers
{
    [Area("Seller")]
    //[Authorize(Roles = "Manager")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IWebHostEnvironment _hostWebEnvironment;
        public ProductController(ApplicationDbContext context, IHostEnvironment hostEnvironment, IWebHostEnvironment hostWebEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _hostWebEnvironment = hostWebEnvironment;
        }

        // GET: Admin/Products
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            //lấy id của thằng đang đăng nhập

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var list = _context.Products.Include(x => x.Category)
                .Where(x => x.ShopId == claim.Value).Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    image = x.ImageUrl,
                    de = x.Description.Count() + " Word",
                    confirm = x.IsConfirm == true ? "VERYFY" : "NOT VERYFIED",
                    price = x.Price,
                    quantity = x.Quantity,
                    cate = x.Category.Name

                }).ToList();

            return Json(new { data = list });
        }
        public IActionResult GetforSelect(string q)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            var obj = _context.Products
                .Include(x => x.Category)
                .Where(x => x.ShopId == claim.Value).ToList()
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ImageUrl = x.ImageUrl,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    CategoryId = x.CategoryId,
                    ShopId = x.ShopId,
                    IsConfirm = x.IsConfirm,

                });
            if (!(string.IsNullOrEmpty(q) || string.IsNullOrWhiteSpace(q)))
            {
                obj = obj.Where(x => x.Name.ToLower().StartsWith(q.ToLower())).ToList();
            }
            return Json(new { items = obj });
        }

        // GET: Accounts/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id", "Name", "Description","ImageUrl", "Price", "IsConfirm", "Quantity", "CategoryId","files")] ProductViewModel productViewModel)
        {
            Product product = new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    IsConfirm = false,
                    Quantity = productViewModel.Quantity,
                    CategoryId = productViewModel.CategoryId,
                };
            //try
            //{


                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (ModelState.IsValid)
                {
                    // save image to wwwroot/image
                    string wwwRootPath = _hostWebEnvironment.WebRootPath;

                    if (productViewModel.files != null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Media\");
                        var extension = Path.GetExtension( productViewModel.files.FileName);

                        if (product.ImageUrl != null)
                        {
                            // edit 
                            var imagePath = Path.Combine(wwwRootPath, product.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            productViewModel.files.CopyTo(filesStreams);
                        }
                        product.ImageUrl = @"\Media\" + fileName + extension;
                    }
                    product.ShopId = claim.Value;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            //}
            //catch
            //{
            //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            //    return View();
            //}
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
            return View(productViewModel);
        }


        public IActionResult Get(string id)
        {
            var product = _context.Products.Include(x=>x.Category).Include(x=>x.Shop).FirstOrDefault(x => x.Id == id);
            
            var obj = new
            {
                id = product.Id,
                name = product.Name,
                description = product.Description,
                price = product.Price,
                quantity = product.Quantity,
                categoryId = product.Category.Name,
                shopId = product.Shop.Name,
                isConfirm = product.IsConfirm,
                imageUrl = product.ImageUrl,

            };
            
            return Json(new { data = obj });
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                IsConfirm = product.IsConfirm,
                Quantity = product.Quantity,
                CategoryId = product.CategoryId,

            };
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", product.CategoryId);

            return View(productViewModel);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id", "Name", "Description","ImageUrl", "Price", "IsConfirm", "Quantity", "CategoryId", "files")] ProductViewModel productViewModel)
        {
            if (id != productViewModel.Id)
            {
                return NotFound();
            }
            Product product = new Product()
            {
                Id = productViewModel.Id,
                Name = productViewModel.Name,
                Description = productViewModel.Description,
                Price = productViewModel.Price,
                IsConfirm = false,
                Quantity = productViewModel.Quantity,
                CategoryId = productViewModel.CategoryId,
            };
            if (ModelState.IsValid)
            {
                try
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                    string wwwRootPath = _hostWebEnvironment.WebRootPath;

                    if ( productViewModel.files!= null)
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"Media\");
                        var extension = Path.GetExtension(productViewModel.files.FileName);

                        if (productViewModel.ImageUrl != null)
                        {
                            // edit 
                            var imagePath = Path.Combine(wwwRootPath, productViewModel.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                        }
                        using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                        {
                            productViewModel.files.CopyTo(filesStreams);
                        }
                        product.ImageUrl = @"\Media\" + fileName + extension;
                    }
                    else
                        product.ImageUrl = productViewModel.ImageUrl;

                    product.ShopId = claim.Value;
                    _context.Products.Update(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch 
                {
                    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
                    return NotFound();
                }
                
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");

            return View(productViewModel);
        }

        [HttpDelete]
        //[ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            try
            {
                var obj = _context.Products.Find(id);
                if(obj == null)
                    return Json(new { success = false, message = "Sản phẩm không tồn tại" });
                string wwwRootPath = _hostWebEnvironment.WebRootPath;

                
                if (obj.ImageUrl != null)
                {
                    // edit 
                    var imagePath = wwwRootPath + obj.ImageUrl;
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                }
                
                _context.Products.Remove(obj);
                _context.SaveChanges();
                return Json(new { success = true, message = "xóa mục thành công" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });

            }
        }

        private bool ProductExists(string id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
   
}

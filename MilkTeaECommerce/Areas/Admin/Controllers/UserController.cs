using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            
        }
        public IActionResult Index()
        {
            
            
            return View();
        }






        public async Task<IActionResult> Create()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await _roleManager.RoleExistsAsync("Manager"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Manager"));
            }
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }
            if (!await _roleManager.RoleExistsAsync("Shipper"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Shipper"));
            }
            var list = _roleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            ViewBag.listRole = list;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = vm.Username,
                    Name = vm.Name,
                    Email = vm.Name,
                    PhoneNumber = vm.Sdt,
                    
                };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, vm.Role);
                    return RedirectToAction(nameof(Index));
                }   
                else
                {
                    foreach(var er in result.Errors)
                    {

                        ModelState.AddModelError("", er.Description);
                    }    
                }    

            }    
            var list = _roleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            ViewBag.listRole = list;
            return View(vm);
        }
        public IActionResult getall()
        {
            var obj = _userManager.Users.Select(x => new
            {
                id = x.Id,
                username = x.UserName,
                email = x.Email,
                sdt = x.PhoneNumber,
                lockout =  _userManager.IsLockedOutAsync(x).Result
            }) ;
            return Json(new { data = obj });
        }
        public async Task<IActionResult> LockUnLock(string id)
        {
            var obj = await _userManager.FindByIdAsync(id);
            var IsLock = await _userManager.IsLockedOutAsync(obj);
            if(IsLock)
            {
                obj.LockoutEnd = DateTimeOffset.Now;
                await _userManager.UpdateAsync(obj);
                return Json(new { success = true, message = "đã mở khóa tài khoản" });
            }
            else
            {
                obj.LockoutEnd = DateTimeOffset.Now.AddYears(1);
                await _userManager.UpdateAsync(obj);
                return Json(new { success = true, message = "đã khóa tài khoản" });
            }    
            

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var obj = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(obj);
            if (result.Succeeded)
            {
               
                return Json(new { success = true, message = "đã xóa tài khoản thành công" });
            }
            else
            {
               
                return Json(new { success =false, message = "Lỗi Khi thực hiện thao tác này" });
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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

        public IActionResult Update(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var u = _userManager.Users.FirstOrDefault(x => x.Id == id);
            if(u== null)
            {
                return NotFound();
            }
            var vm = new RegisterViewModel() { Username = u.UserName, Mail = u.Email, Name = u.Name, Sdt = u.PhoneNumber };
            vm.Role = _userManager.GetRolesAsync(u).Result.ToArray();
            var list = _roleManager.Roles.Select(x => new SelectListItem() { Text = x.Name, Value = x.Name }).ToList();
            ViewBag.listRole = list;
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(RegisterViewModel vm)
        {
            
            var u = _userManager.Users.FirstOrDefault(x=>x.UserName == vm.Username);
            if(u == null)
            {
                ModelState.AddModelError("","Lỗi");
                return View(vm);
            }
            try
            {
                var rolelist = await _userManager.GetRolesAsync(u);
                await _userManager.RemoveFromRolesAsync(u, rolelist);
                await _userManager.AddToRolesAsync(u, vm.Role);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(vm);
            }
            return RedirectToAction("index");
           
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
                    Email = vm.Mail,
                    PhoneNumber = vm.Sdt,
                    
                };
                var result = await _userManager.CreateAsync(user, vm.Password);
                if(result.Succeeded)
                {
                    await _userManager.AddToRolesAsync(user, vm.Role);
                    return RedirectToAction("index","home",new {area ="admin" });
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

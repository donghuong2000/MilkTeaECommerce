using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Controllers
{
    
    public class IdentityController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string username,string password)
        {
            var obj = await _signInManager.PasswordSignInAsync(username, password,false, lockoutOnFailure: false);
            if(obj.Succeeded)
            {
                return Json(new { success = true, message = "Đăng nhập thành công" });
            } 
            if(obj.IsLockedOut)
            {
                return Json(new { success = false, message = "Account was Lockout" });
            }
            return Json(new { success = false, message = "Account ko hợp lệ" });

        }

        [HttpPost]
        public async Task<IActionResult> SignUp(string name,string email,string sdt,string username,string password)
        {

            var user = new ApplicationUser() { Name = name, Email = email, PhoneNumber = sdt, UserName = username };
            var result = await _userManager.CreateAsync(user,password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _signInManager.SignInAsync(user, false);
                return Json(new { success = true, message = "SignUp Success" });
            }
            string er = "";
            foreach (var item in result.Errors)
            {
                er += item.Description + "\n";
            }
            return Json(new { success = false, message = er });


        }
        public async Task<IActionResult> Logout()
        {


            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");


        }
    }
}

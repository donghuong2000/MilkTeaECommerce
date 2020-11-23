using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using MilkTeaECommerce.Models;
using MilkTeaECommerce.Models.Models;

namespace MilkTeaECommerce.Controllers
{
    
    public class IdentityController : Controller
    {
        private static string _returnUrl;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        public IdentityController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(string username,string password)
        {
            var obj = await _signInManager.PasswordSignInAsync(username, password,false, lockoutOnFailure: false);
            if(obj.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(userName: username);
                var role = await _userManager.GetRolesAsync(user);
                if (role.Contains("Admin")) 
                {
                    return Json(new { success = true, message = "Admin", url ="/Admin/Home" });
                }    
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
                // xác nhận mail 
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(
                    "ConfirmEmail",
                    "Identity",
                    values: new { userId = user.Id, code = code},
                    protocol: Request.Scheme,
                    host: Request.Host.Value);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                await _userManager.AddToRoleAsync(user, "Customer");
                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                {
                    return Json(new { success = false, message = "comfirm email " });
                }
                else
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Json(new { success = true, message = "SignUp Success" });

                }

            }
            string er = "";
            foreach (var item in result.Errors)
            {
                er += item.Description + "\n";
            }
            return Json(new { success = false, message = er });


        }
        


        public async Task<IActionResult> ConFirmEmail(string userId,string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index","home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
            {
                return Content("đã xác nhận email thành công");
            }    
            else
            {
                return Content("đã xác nhận email không thành công");
            }    
            
        }


        public async Task<IActionResult> Logout()
        {


            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");


        }


        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            _returnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _returnUrl = _returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    
                    return LocalRedirect(_returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = _returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                   
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public IActionResult AccessDenied()
        {
            
            return View();
        }

        public IActionResult LockOut()
        {

            return View();
        }


    }
}

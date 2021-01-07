using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MilkTeaECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MilkTeaECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShipperController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public ShipperController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAll()
        {
            var obj = _userManager.Users
                .Where(x => x.ShipperRequest != ShipperRequest.None)
                .Select(x => new
                {
                    id = x.Id,
                    username = x.UserName,
                    email = x.Email,
                    sdt = x.PhoneNumber,
                    lockout = x.ShipperRequest
                });
            return Json(new { data = obj });
        }
        public async Task<IActionResult> ChangeStatus(string id,int status)
        {
            try
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                if(status == 2)
                {
                    await _userManager.AddToRoleAsync(user,"Shipper");
                    user.ShipperRequest = ShipperRequest.Approved;
                }    
                if(status == 3)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Shipper");
                    user.ShipperRequest = ShipperRequest.Block;
                }

                await _userManager.UpdateAsync(user);
                return Json(new { success = true, message = "thay đổi trạng thái shipper thành công" });
            }
            catch (Exception e )
            {

                return Json(new { success = false, message = e.Message });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MilkTeaECommerce.Data;
using MilkTeaECommerce.Models;

namespace MilkTeaECommerce.Controllers
{
    public class RatingController : Controller
    {
        public readonly ApplicationDbContext _db;
        public RatingController (ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Creat(string productid, string userid, string content,float? rate)
        {
            try
            {
                Rating rating = new Rating() {  }
            }
        }
    }
}

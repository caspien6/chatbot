using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Models;
using BLL.Context;
using BLL.Models.Game;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Controllers
{
    public class HomeController : Controller
    {
        private StoryContext _context;

        public HomeController(StoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           return Ok(_context.Users);
            

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

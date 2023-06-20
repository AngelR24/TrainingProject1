using Microsoft.AspNetCore.Mvc;
using MVCProject2.Models;
using System.Diagnostics;

namespace MVCProject2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(@"../Users/UsersView");
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
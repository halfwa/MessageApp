using MessageApp.WEB.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace MessageApp.WEB.Controllers
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
            return View();
        }

        public IActionResult Send()
        {
            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult RealTimeList()
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using notification_system.Interfaces;
using notification_system.Models;
using notification_system.Services;
using System.Diagnostics;

namespace notification_system.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private IUserLogin _userLogin;
        private ExpirationService _service;
        public HomeController(IUserLogin userLogin, ILogger<HomeController> logger, ExpirationService service) {
            _logger = logger;
            _userLogin = userLogin;
            _service = service;
        }

        public IActionResult Index() {
            return View();
        }
/*
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Index(string username, string password) {
           *//* var isSuccess = await _userLogin.AuthenticateUser(username, password);

            if (isSuccess != null) {
                TempData["UserId"] = isSuccess.Id;
                await _service.StartAsync();*//*
                return RedirectToAction("Index", "Requests");
         *//*   }
            else {
                ViewBag.username = string.Format("Login Failed ", username);
                return View();
            }*//*
        }*/

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
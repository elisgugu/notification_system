using common_data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using common_data.Models;
//using notification_system.Services;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;

namespace notification_center_app.Controllers
{
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger) {
            _logger = logger;
        }

        public IActionResult Index() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string username, string password)
        {
            using (var httpClient = new HttpClient())
            {
                var userLogin = new User();
                userLogin.UserName = username;
                userLogin.Password = password;

                HttpResponseMessage response = await httpClient.PostAsJsonAsync<User>("https://localhost:7187/api/Login", userLogin);
                if (response.IsSuccessStatusCode)
                {
                    userLogin = await response.Content.ReadAsAsync<User>();
                   
                    if (userLogin != null)
                    {
                        TempData["UserId"] = userLogin.Id;
                        return RedirectToAction("Index", "Requests");
                    }
                }
                ViewBag.username = string.Format("Login Failed ", username);
                return View();
            }
        }

        public IActionResult Privacy() {
            return View();
        }

      /*  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
           // return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }*/
    }
}
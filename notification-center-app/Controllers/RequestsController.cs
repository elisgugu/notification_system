using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using common_data.Data;

namespace notification_center_app.Controllers
{
    public class RequestsController : Controller {
        public RequestsController() {
          
        }
        public IActionResult Index() {
         
            return View();

        }
    }
}

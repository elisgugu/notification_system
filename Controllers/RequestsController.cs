using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;

namespace notification_system.Controllers {
    public class RequestsController : Controller {
        private readonly NotificationCenterContext _context;
        public RequestsController(NotificationCenterContext context) {
            _context = context;


        }
        public IActionResult Index() {
            return View();

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;
using System.Security.Claims;

namespace notification_system.Controllers {
    public class RequestsController : Controller {
        private readonly NotificationCenterContext _context;
        public RequestsController(NotificationCenterContext context, IHttpContextAccessor httpContextAccessor) {
          
            _context = context;
        }
        public IActionResult Index() {
         
            return View();

        }
    }
}

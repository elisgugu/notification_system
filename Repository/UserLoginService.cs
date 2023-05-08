using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Interfaces;
using notification_system.Models;
using System.Security.Claims;

namespace notification_system.Repository
{
    public class UserLoginService : IUserLogin {
        private readonly NotificationCenterContext _context;
       // private readonly IHubContext<SignalRServer> _context;
        public UserLoginService(NotificationCenterContext context) {
            _context = context;
          
        }
        public async Task<User> AuthenticateUser(string username, string passcode) {
            var succeeded = await _context.Users.FirstOrDefaultAsync(authUser => authUser.UserName == username && authUser.Password == passcode);
            
            return succeeded;
        }

    }
}

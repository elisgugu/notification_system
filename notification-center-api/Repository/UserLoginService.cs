using common_data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using common_data.Interfaces;
using System.Security.Claims;
using common_data.Data;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace notification_center_api.Repository
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

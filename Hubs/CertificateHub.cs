using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;
using notification_system.Repository;
using System.Collections.Concurrent;
using TableDependency.SqlClient.Base.EventArgs;

namespace notification_system.Hubs {

    public class CertificateHub : Hub {
        IHttpContextAccessor _httpContextAccessor;
      
        public CertificateHub(IHttpContextAccessor httpContextAccessor) {
            _httpContextAccessor = httpContextAccessor;
      
        }

        public override async Task OnConnectedAsync() {
   
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex) {
    
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);
        }
    }
}

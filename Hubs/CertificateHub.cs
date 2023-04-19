using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;
using notification_system.Repository;
using TableDependency.SqlClient.Base.EventArgs;

namespace notification_system.Hubs {

    public class CertificateHub : Hub {
        public CertificateHub() {
           
        }
        public async Task SendRequests(List<Certificate> certificates) {
            await Clients?.All.SendAsync("certificatesExpired", certificates);
        }
    }
}

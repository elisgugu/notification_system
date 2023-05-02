using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;
using notification_system.Repository;
using System.Collections.Concurrent;
using System.Security.Claims;
using TableDependency.SqlClient.Base.EventArgs;

namespace notification_system.Hubs {

    public class RequestHub : Hub {
        RequestRepository _requestRepository;
        IHttpContextAccessor _httpContextAccessor;
        private static ConcurrentDictionary<string, string> clients = new ConcurrentDictionary<string, string>();

        public RequestHub(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) {
            _requestRepository = new RequestRepository(configuration);
            _httpContextAccessor = httpContextAccessor;
            clients = new ConcurrentDictionary<string, string>();
        }

        public override async Task OnConnectedAsync() {
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex) {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
            await base.OnDisconnectedAsync(ex);
        }

        public string Login(string username) {
            clients.TryAdd(Context.ConnectionId, username);
            return username;
        }


        public async Task SendRequests(string userId) {
            var requests = _requestRepository.GetAllRequests(userId);
            await Clients.Caller.SendAsync("receivedRequest", requests);
        }

        internal async void SendUpdatedRecord(RecordChangedEventArgs<Request> e) {
           if (e.EntityOldValues.StatusId != e.Entity.StatusId) {
                var requests = _requestRepository.GetAllRequests(e.Entity.Id.ToString());
                var changedRequest = requests.FirstOrDefault(x => x.Id == e.Entity.Id);
                await Clients.Group(Context.User.Identity.Name)?.SendAsync("statusChanged", changedRequest);
            }
        }
    }
}

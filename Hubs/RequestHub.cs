using Microsoft.AspNetCore.SignalR;
using notification_system.Interfaces;
using notification_system.Models;
using notification_system.Repository;
using System.Collections.Concurrent;
using TableDependency.SqlClient.Base.EventArgs;

namespace notification_system.Hubs {

    public class RequestHub : Hub {
        IRequestRepository _requestRepository;
        IHttpContextAccessor _httpContextAccessor;
        private static ConcurrentDictionary<string, string> clients = new ConcurrentDictionary<string, string>();

        public RequestHub(IRequestRepository reqRepo, IHttpContextAccessor httpContextAccessor) {
            _requestRepository = reqRepo;
            
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


        public async Task SendRequests() {
            var userId = _httpContextAccessor.HttpContext?.User?.Claims?.FirstOrDefault().ToString();
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

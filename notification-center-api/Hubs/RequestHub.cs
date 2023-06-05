using common_data.Models;
using Microsoft.AspNetCore.SignalR;
using common_data.Interfaces;
using notification_center_api.Repository;
using System.Collections.Concurrent;
using TableDependency.SqlClient.Base.EventArgs;
using System.Security.Claims;

namespace notification_center_api.Hubs {

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
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await Groups.AddToGroupAsync(Context.ConnectionId, userName);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex) {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userName);
            await base.OnDisconnectedAsync(ex);
        }

        public string Login(string username) {
            clients.TryAdd(Context.ConnectionId, username);
            return username;
        }


        public async Task SendRequests() {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var requests = _requestRepository.GetAllRequests(userName);
            await Clients.Caller.SendAsync("receivedRequest", requests);
        }

        internal async void SendUpdatedRecord(RecordChangedEventArgs<Request> e) {
           if (e.EntityOldValues.StatusId != e.Entity.StatusId) {            
                var requests = _requestRepository.GetAllRequests();
                var changedRequest = requests.FirstOrDefault(x => x.Id == e.Entity.Id);
                await Clients.Group(Context.User.Identity.Name)?.SendAsync("statusChanged", changedRequest);
            }
        }
    }
}

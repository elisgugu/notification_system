using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using notification_system.Models;
using notification_system.Repository;
using TableDependency.SqlClient.Base.EventArgs;

namespace notification_system.Hubs {

    public class RequestHub : Hub {
        RequestRepository _requestRepository;
        public RequestHub(IConfiguration configuration, IUserIdProvider userProvider) {
            _requestRepository = new RequestRepository(configuration);
        }
       
        public async Task SendRequests() {
            var requests = _requestRepository.GetAllRequests();
            await Clients.Caller.SendAsync("receivedRequest", requests);
        }

        internal async void SendUpdatedRecord(RecordChangedEventArgs<Request> e) {
           if (e.EntityOldValues.StatusId != e.Entity.StatusId) {
                var requests = _requestRepository.GetAllRequests();
                var changedRequest = requests.FirstOrDefault(x => x.Id == e.Entity.Id);
                await Clients.Caller.SendAsync("statusChanged", changedRequest);
            }
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using notification_system.Data;
using notification_system.Hubs;
using notification_system.Interfaces;
using NuGet.Protocol.Core.Types;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using Request = notification_system.Models.Request;

namespace notification_system.SubscribeTableDependencies
{
    public class SubscribeRequestTableDependency : ISubscribeTableDependency {
        SqlTableDependency<Models.Request> _tableDependency;
        IHubContext<RequestHub> _requestHub;
        IServiceScopeFactory _factory;



        public SubscribeRequestTableDependency(IServiceScopeFactory factory, IHubContext<RequestHub> requestHub) {
            _requestHub = requestHub;
            _factory = factory;
        }

        public void SubscribeTableDependency(string connectionString) {
            var mapper = new ModelToTableMapper<Request>();
            mapper.AddMapping(model => model.StatusId, "status_id");

           

            _tableDependency = new SqlTableDependency<Request>(connectionString, "Request", null,mapper, includeOldValues: true);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Request> e) {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None) {
                //_requestHub.SendRequests().Wait();
            }
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update) {
                if (e.EntityOldValues.StatusId != e.Entity.StatusId) {
                    await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                    var context = asyncScope.ServiceProvider.GetRequiredService<NotificationCenterContext>();

                    var repository = asyncScope.ServiceProvider.GetRequiredService<IRequestRepository>();
                               var requests = repository.GetAllRequests(e.Entity.Id.ToString());
                               var changedRequest = requests.FirstOrDefault(x => x.Id == e.Entity.Id);
                               await _requestHub.Clients.All.SendAsync("statusChanged", changedRequest);
                }
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e) {
            Console.WriteLine($"{nameof(Request)} SqlTableDependency error: {e.Error.Message}");
        }
    }

}

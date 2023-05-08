using Microsoft.AspNetCore.SignalR;
using notification_system.Hubs;
using notification_system.Models;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;

namespace notification_system.SubscribeTableDependencies {
    public class SubscribeRequestTableDependency : ISubscribeTableDependency {
        SqlTableDependency<Request> _tableDependency;
        RequestHub _requestHub;
        

        public SubscribeRequestTableDependency() {
     
        }

        public void SubscribeTableDependency(string connectionString, RequestHub requestHub) {
            var mapper = new ModelToTableMapper<Request>();
            mapper.AddMapping(model => model.StatusId, "status_id");

            _requestHub = requestHub;

            _tableDependency = new SqlTableDependency<Request>(connectionString, "Request", null,mapper, includeOldValues: true);
            _tableDependency.OnChanged += TableDependency_OnChanged;
            _tableDependency.OnError += TableDependency_OnError;
            _tableDependency.Start();
        }

        private void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Request> e) {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None) {
                //_requestHub.SendRequests().Wait();
            }
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update) {
                _requestHub.SendUpdatedRecord(e);
            }
        }

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e) {
            Console.WriteLine($"{nameof(Request)} SqlTableDependency error: {e.Error.Message}");
        }
    }

}

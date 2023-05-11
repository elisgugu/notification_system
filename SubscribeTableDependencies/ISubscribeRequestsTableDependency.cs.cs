using Microsoft.AspNetCore.SignalR;
using notification_system.Hubs;

namespace notification_system.SubscribeTableDependencies {
    public interface ISubscribeTableDependency {
        void SubscribeTableDependency(string connectionString);
    }
}

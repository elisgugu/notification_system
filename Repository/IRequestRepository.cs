using notification_system.Models;

namespace notification_system.Repository {
    public interface IRequestRepository {
        List<Request> GetAllRequests();
    }
}

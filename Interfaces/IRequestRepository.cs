using notification_system.Models;

namespace notification_system.Interfaces
{
    public interface IRequestRepository
    {
        List<Request> GetAllRequests(string id);
    }
}

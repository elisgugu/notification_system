using common_data.Models;

namespace common_data.Interfaces
{
    public interface IRequestRepository
    {
        List<Request> GetAllRequests(string id);
        List<Request> GetAllRequests();
    }
}

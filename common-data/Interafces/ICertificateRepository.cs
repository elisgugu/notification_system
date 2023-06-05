
using common_data.Models;
using common_data.Data;

namespace common_data.Interfaces
{
    public interface ICertificateRepository
    {
        Task<List<Certificate>> GetExpiredCertificates(NotificationCenterContext dbContext);
    }
}

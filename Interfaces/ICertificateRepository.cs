using Microsoft.EntityFrameworkCore;
using notification_system.Data;
using notification_system.Models;

namespace notification_system.Interfaces
{
    public interface ICertificateRepository
    {
        Task<List<Certificate>> GetExpiredCertificates(NotificationCenterContext dbContext);
    }
}

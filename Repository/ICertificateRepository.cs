using notification_system.Models;

namespace notification_system.Repository {
    public interface ICertificateRepository {
        List<Certificate> GetExpiredCertificates();
    }
}

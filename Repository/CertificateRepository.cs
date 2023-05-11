using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using notification_system.Interfaces;
using notification_system.Models;
using System.Security.Claims;
using System.Security.Principal;

namespace notification_system.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        IHttpContextAccessor _httpContextAccessor;
        private readonly NotificationCenterContext _dbContext;

        public CertificateRepository(NotificationCenterContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Certificate>> GetExpiredCertificates(NotificationCenterContext dbContext)
        {
            var certifications = new List<Certificate>();
            var userCertificates = from r in dbContext.Certificates
                                    join rs in dbContext.Users on r.UserId equals rs.Id
                                    where DateTime.Compare(DateTime.Now, r.EndDate) > 0
                                    select new { r, rs.UserName, rs.Name };

            await userCertificates.ForEachAsync((certificate) => {
                certifications.Add(new Certificate() {
                    User = new User() {
                        UserName = certificate.UserName,
                        Name = certificate.Name
                    },
                    Id = certificate.r.Id,
                    EndDate = certificate.r.EndDate,
                    UserId = certificate.r.UserId
                });
            });

            return certifications;
        }
    }
}

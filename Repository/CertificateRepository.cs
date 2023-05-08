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

            /* using (SqlConnection conn = new SqlConnection(connectionString))
             {
                 conn.Open();

                 string commandText = "select * from certificate JOIN [user]  ON certificate.user_id = [user].id  where datediff(dd, end_date, getdate())>= 1";

                 SqlCommand cmd = new(commandText, conn);

                 var reader = cmd.ExecuteReader();

                 while (reader.Read())
                 {
                     var certificate = new Certificate
                     {
                         Id = (int)reader["id"],
                         User = new User()
                         {
                             Name = reader["name"].ToString(),
                         },
                         EndDate = Convert.ToDateTime(reader["end_date"]),
                     };
                     certifications.Add(certificate);
                 }
             }*/
            var userCertificates = from r in dbContext.Certificates
                                    join rs in dbContext.Users on r.UserId equals rs.Id
                                    where DateTime.Compare(DateTime.Now, r.EndDate) > 0
                                    select new { r, rs.UserName };

            await userCertificates.ForEachAsync((certificate) => {
                certifications.Add(new Certificate() {
                    User = certificate.r.User,
                    Id = certificate.r.Id,
                    EndDate = certificate.r.EndDate,
                });
            });

            return certifications;
        }
    }
}

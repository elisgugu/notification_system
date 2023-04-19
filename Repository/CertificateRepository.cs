﻿using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Data.SqlClient;
using notification_system.Models;

namespace notification_system.Repository {
    public class CertificateRepository : ICertificateRepository {
       

        string connectionString = "";

        public CertificateRepository(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public List<Certificate> GetExpiredCertificates() {
            var certifications = new List<Certificate>();

            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();

                SqlDependency.Start(connectionString);

                string commandText = "select * from certificate JOIN [user]  ON certificate.user_id = [user].id  where datediff(dd, end_date, getdate())>= 1";

                SqlCommand cmd = new(commandText, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var certificate = new Certificate {
                        Id = Int32.Parse(reader["id"].ToString()),
                        User = new User() {
                            Name = reader["name"].ToString(),
                        },
                        EndDate = DateTime.Parse(reader["end_date"].ToString()),
                    };
                    certifications.Add(certificate);
                }
            }

            return certifications;
        }
    }
}
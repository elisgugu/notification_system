using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using notification_system.Hubs;
using notification_system.Models;

namespace notification_system.Repository {
    public class RequestRepository : IRequestRepository {
        string connectionString = "";

        public RequestRepository(IConfiguration configuration) {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Request> GetAllRequests(string id) {
            var notifications = new List<Request>();
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();

                SqlDependency.Start(connectionString);

                string commandText = $"SELECT request.id as id, date, name, status FROM request JOIN request_status  ON request.status_id = request_status.id JOIN [user]  ON dbo.[user].id = request.user_id" +
                    $" where dbo.[user].id = '{id}'";

                
                SqlCommand cmd = new (commandText, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var employee = new Request {
                        Id = Guid.Parse(reader["id"].ToString()),
                        Status = new RequestStatus() {
                            Status = reader["status"].ToString(),
                        },
                        Date = DateTime.Parse(reader["date"].ToString()),
                        User = new User() {
                            Name = reader["name"].ToString(),
                        }
                        
                    };

                    notifications.Add(employee);
                }
            }
            return notifications;
        }
    }
}

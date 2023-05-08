using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using notification_system.Hubs;
using notification_system.Interfaces;
using notification_system.Models;

namespace notification_system.Repository
{
    public class RequestRepository : IRequestRepository {
        private readonly NotificationCenterContext _dbContext;

        public RequestRepository(NotificationCenterContext dbContext) {
            _dbContext = dbContext;
        }

        public List<Request> GetAllRequests(string id) {
               var notifications = new List<Request>();
        /*    var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (SqlConnection conn = new SqlConnection(connectionString)) {
                conn.Open();

                string commandText = $"SELECT request.id as id, date, name, status FROM request JOIN request_status  ON request.status_id = request_status.id JOIN [user]  ON dbo.[user].id = request.user_id" +
                    $" where dbo.[user].id = '{id}'";


                SqlCommand cmd = new(commandText, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read()) {
                    var employee = new Request {
                        Id = (Guid)reader["id"],
                        Status = new RequestStatus() {
                            Status = reader["status"].ToString(),
                        },
                        Date = System.Convert.ToDateTime(reader["date"]),
                        User = new User() {
                            Name = reader["name"].ToString(),
                        }

                    };

                    notifications.Add(employee);
                }
            }*/

            var userNotifications = from r in _dbContext.Requests join rs in _dbContext.RequestStatuses on r.StatusId equals rs.Id
                                    join u in _dbContext.Users on r.UserId equals u.Id 
                                    select new { r };

            foreach (var item in userNotifications) {

                notifications.Add(new Request() {
                     Date = item.r.Date,
                     Id = item.r.Id,
                     Status = item.r.Status,
                });
            }
               
            return notifications;
        }
    }
}

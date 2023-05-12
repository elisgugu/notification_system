using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using notification_system.Data;
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
            var userNotifications = from r in _dbContext.Requests join rs in _dbContext.RequestStatuses on r.StatusId equals rs.Id
                                    join u in _dbContext.Users on r.UserId equals u.Id 
                                    select new { r, u.UserName, u.Name, rs.Status };

            foreach (var item in userNotifications) {

                notifications.Add(new Request() {
                     Date = item.r.Date,
                     Id = item.r.Id,
                     Status = new RequestStatus() {
                         Status = item.Status
                     },
                     User = new User() {
                         UserName = item.UserName,
                         Name = item.Name
                     }
                });
            }
               
            return notifications;
        }
    }
}

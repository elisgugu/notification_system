using common_data.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using common_data.Data;
using notification_center_api.Hubs;
using common_data.Interfaces;
using System.Security.Claims;

namespace notification_center_api.Repository
{
    public class RequestRepository : IRequestRepository {
        private readonly NotificationCenterContext _dbContext;
        IHttpContextAccessor _httpContextAccessor;

        public RequestRepository(NotificationCenterContext dbContext, IHttpContextAccessor httpContextAccessor) {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public List<Request> GetAllRequests() {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var notifications = new List<Request>();
            var userNotifications = from r in _dbContext.Requests join rs in _dbContext.RequestStatuses on r.StatusId equals rs.Id
                                    join u in _dbContext.Users on r.UserId equals u.Id
                                    where u.UserName == userName
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

        public List<Request> GetAllRequests(string id)
        {
            throw new NotImplementedException();
        }
    }
}

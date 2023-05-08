using notification_system.Models;

namespace notification_system.Interfaces
{
    public interface IUserLogin
    {
        Task<User> AuthenticateUser(string username, string password);
    }
}

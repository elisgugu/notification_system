using common_data.Models;

namespace common_data.Interfaces
{
    public interface IUserLogin
    {
        Task<User> AuthenticateUser(string username, string password);
    }
}

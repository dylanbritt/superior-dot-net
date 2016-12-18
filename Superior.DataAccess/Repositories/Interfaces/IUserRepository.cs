using Superior.Domain.Models;

namespace Superior.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User GetUserByUserName(string userName);

        void CreateUser(User user);

        void UpdateUserLoginMonitor(UserLoginMonitor userLoginMonitor);
    }
}

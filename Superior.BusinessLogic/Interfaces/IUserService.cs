using Superior.Domain.Enums;
using Superior.Domain.Models;

namespace Superior.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        void CreateUser(User user);
        AuthenticationCode AuthenticateUser(User user);
    }
}

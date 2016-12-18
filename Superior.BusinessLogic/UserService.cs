using Superior.BusinessLogic.Interfaces;
using Superior.DataAccess.Repositories.Interfaces;
using Superior.Domain.Enums;
using Superior.Domain.Models;
using Superior.Utility;
using System;

namespace Superior.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null)
                throw new ArgumentNullException(nameof(userRepository));

            _userRepository = userRepository;
        }

        public void CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.UserCredential == null)
                throw new ArgumentNullException(nameof(user.UserCredential));

            if (user.UserCredential.Salt == null)
                user.UserCredential.Salt = EncryptionUtility.Generate32ByteSalt();

            if (user.UserCredential.EncryptedPassword == null)
                user.UserCredential.EncryptPassword();

            user.UserLoginMonitor = new UserLoginMonitor
            {
                UtcDateTimeLastLoginAttempt = DateTime.UtcNow
            };

            _userRepository.CreateUser(user);
        }

        public AuthenticationCode AuthenticateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (user.UserName == null)
                throw new ArgumentNullException(nameof(user.UserName));
            if (user.UserCredential == null)
                throw new ArgumentNullException(nameof(user.UserCredential));
            if (user.UserCredential.Password == null)
                throw new ArgumentNullException(nameof(user.UserCredential.Password));

            bool isAuthenticated = false;
            var authenticUser = _userRepository.GetUserByUserName(user.UserName);

            if (authenticUser != null)
            {
                if (authenticUser.UserLoginMonitor.CanLogin())
                {
                    isAuthenticated =
                        authenticUser.UserLoginMonitor.AuthenticateUser(user);

                    _userRepository.UpdateUserLoginMonitor(authenticUser.UserLoginMonitor);
                }

                if (!authenticUser.UserLoginMonitor.CanLogin())
                    return AuthenticationCode.Locked;
            }

            return isAuthenticated
                ? AuthenticationCode.Authenticated
                    : AuthenticationCode.Unauthenticated;
        }
    }
}

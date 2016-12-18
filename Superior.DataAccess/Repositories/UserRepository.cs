using Superior.DataAccess.Context;
using Superior.DataAccess.Repositories.Interfaces;
using Superior.Domain.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace Superior.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByUserName(string userName)
        {
            using (var db = new SuperiorContext())
            {
                return
                    db.Users
                        .Include(uc => uc.UserCredential)
                        .Include(ulm => ulm.UserLoginMonitor)
                        .SingleOrDefault(u =>
                            string.Equals(u.UserName.ToLower(), userName.ToLower()));
            }
        }

        public void CreateUser(User user)
        {
            using (var db = new SuperiorContext())
            {
                if (user.UserCredential == null)
                    throw new ArgumentNullException("user.UserCredential");

                if (user.UserCredential.Salt == null)
                    throw new ArgumentNullException("user.UserCredential.Salt");

                if (user.UserCredential.EncryptedPassword == null)
                    throw new ArgumentNullException("user.UserCredential.EncryptedPassword");

                if (db.Users.Any(u => string.Equals(u.UserName.ToLower(), user.UserName.ToLower())))
                    throw new ArgumentException($"Cannot create User with UserName {user.UserName}. {user.UserName} is an existing UserName.");

                user.UserId = Guid.NewGuid();
                user.UtcDateTimeCreated = DateTime.UtcNow;
                user.UserCredential.UtcDateTimeCreated = DateTime.UtcNow;

                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void UpdateUserLoginMonitor(UserLoginMonitor userLoginMonitor)
        {
            if (userLoginMonitor == null)
                throw new ArgumentNullException(nameof(userLoginMonitor));

            using (var db = new SuperiorContext())
            {
                if (!db.UserLoginMonitors
                    .Any(ulm =>
                        ulm.UserLoginMonitorId == userLoginMonitor.UserLoginMonitorId))
                    throw new InvalidOperationException(nameof(userLoginMonitor));

                db.Entry(userLoginMonitor).State = EntityState.Modified;

                db.SaveChanges();
            }
        }
    }
}

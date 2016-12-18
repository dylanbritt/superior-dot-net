using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Superior.Domain.Models
{
    public class UserLoginMonitor
    {
        public UserLoginMonitor()
        {

        }

        [ForeignKey("User")]
        public Guid UserLoginMonitorId { get; set; }

        [Required]
        [Range(0, short.MaxValue)]
        public short LoginAttemptCount { get; set; }

        [Required]
        public DateTime UtcDateTimeLastLoginAttempt { get; set; }

        public virtual User User { get; set; }

        public bool CanLogin()
        {
            // greater than 5 minutes
            if ((DateTime.UtcNow - UtcDateTimeLastLoginAttempt).TotalSeconds > 300)
                LoginAttemptCount = 0;

            return LoginAttemptCount < 5 ||
                (DateTime.UtcNow - UtcDateTimeLastLoginAttempt).TotalSeconds > 30;
        }

        public bool AuthenticateUser(User anonymousUser)
        {
            if (anonymousUser == null)
                throw new ArgumentNullException(nameof(anonymousUser));
            if (anonymousUser.UserCredential == null)
                throw new ArgumentNullException(nameof(anonymousUser.UserCredential));
            if (anonymousUser.UserCredential.Password == null)
                throw new ArgumentNullException(nameof(anonymousUser.UserCredential.Password));
            if (User == null)
                throw new InvalidOperationException(nameof(User));
            if (User.UserCredential == null)
                throw new InvalidOperationException(nameof(User.UserCredential));

            var password = anonymousUser.UserCredential.Password;

            bool isAuthenticated =
                User.UserCredential.IsEncryptedPasswordEqualTo(password);

            if (isAuthenticated)
            {
                LoginAttemptCount = 0;
            }
            else
            {
                LoginAttemptCount++;
            }

            UtcDateTimeLastLoginAttempt = DateTime.UtcNow;

            return isAuthenticated;
        }
    }
}

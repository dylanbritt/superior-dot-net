using Superior.Utility;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Superior.Domain.Models
{
    public class UserCredential
    {
        public UserCredential()
        {

        }

        [ForeignKey("User")]
        public Guid UserCredentialId { get; set; }

        [Required]
        [MaxLength(64)]
        [NotMapped]
        public string Password { get; set; }

        [Required]
        [StringLength(256)]
        public byte[] EncryptedPassword { get; set; }

        [Required]
        [StringLength(64)]
        public byte[] Salt { get; set; }

        [Required]
        public DateTime? UtcDateTimeCreated { get; set; }

        public virtual User User { get; set; }

        // TODO: Unit Test
        public void EncryptPassword()
        {
            if (Password == null)
                throw new InvalidOperationException(nameof(Password));

            if (Salt == null)
            {
                Salt = EncryptionUtility.Generate64ByteSalt();
            }

            EncryptedPassword = EncryptionUtility.PBKDF2(Password, Salt);
        }

        // TODO: Unit Test
        public bool IsEncryptedPasswordEqualTo(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (EncryptedPassword == null)
                throw new InvalidOperationException(nameof(EncryptedPassword));

            var otherCredential = new UserCredential { Password = password, Salt = this.Salt };
            otherCredential.EncryptPassword();

            return
                EncryptedPassword
                    .SequenceEqual(otherCredential.EncryptedPassword);
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Superior.Domain.Models
{
    public class User
    {
        public User()
        {

        }

        public Guid UserId { get; set; }

        [Required]
        [MaxLength(64)]
        public string UserName { get; set; }

        [Required]
        public DateTime? UtcDateTimeCreated { get; set; }

        public virtual UserCredential UserCredential { get; set; }

        public virtual UserLoginMonitor UserLoginMonitor { get; set; }
    }
}

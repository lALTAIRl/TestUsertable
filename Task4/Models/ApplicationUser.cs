using System;

namespace Task4.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string Status { get; set; }

        public string PasswordHash { get; set; }
    }
}

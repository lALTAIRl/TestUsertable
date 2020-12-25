using System.ComponentModel.DataAnnotations;

namespace Task4.ViewModels
{
    public class UserRegisterViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordConfirmation { get; set; }
    }
}

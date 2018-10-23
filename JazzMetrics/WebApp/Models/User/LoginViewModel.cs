using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.User
{
    public class LoginViewModel : ViewModel
    {
        [Required]
        [Display(Name = "Uživatelské jméno")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Heslo")]
        public string Password { get; set; }
    }
}
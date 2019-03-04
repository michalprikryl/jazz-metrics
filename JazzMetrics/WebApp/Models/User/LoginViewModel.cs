using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.User
{
    public class LoginViewModel : ViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required!")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }
    }
}

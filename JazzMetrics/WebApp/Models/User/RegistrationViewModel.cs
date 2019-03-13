using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.User
{
    public class RegistrationViewModel : UserBaseModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm password, please!")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "I want create new company - (if not, admin from existing company will assign you to their company).")]
        public bool CreateCompany { get; set; }

        [Display(Name = "New company name")]
        public string Company { get; set; }

        public int? CompanyId { get; set; }
    }
}

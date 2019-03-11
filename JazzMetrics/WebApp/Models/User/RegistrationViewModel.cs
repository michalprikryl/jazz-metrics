using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.User
{
    public class RegistrationViewModel : ViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!")]
        public string Password { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is required!")]
        public string Firstname { get; set; }

        [Display(Name = "Lastname")]
        [Required(ErrorMessage = "Lastname is required!")]
        public string Lastname { get; set; }

        [Display(Name = "Language")]
        [Required(ErrorMessage = "Choose Language, please!")]
        public string LanguageId { get; set; }
        public List<SelectListItem> Languages { get; set; }

        [Display(Name = "I want use LDAP login")]
        public bool UseLdaplogin { get; set; }

        [Display(Name = "LDAP URL")]
        public string LdapUrl { get; set; }

        [Display(Name = "I want create new company - (if not, admin from existing company will assign you to their company).")]
        public bool CreateCompany { get; set; }

        [Display(Name = "New company name")]
        public string Company { get; set; }

        public int? CompanyId { get; set; }
    }
}

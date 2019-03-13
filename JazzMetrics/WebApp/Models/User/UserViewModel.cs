using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.User
{
    public class UserViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int LanguageId { get; set; }
        public int UserRoleId { get; set; }
        public int? CompanyId { get; set; }
        public bool UseLdaplogin { get; set; }
        public string LdapUrl { get; set; }
        public bool Admin { get; set; }
    }

    public class UserBaseModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required!")]
        public string Email { get; set; }

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
    }

    public class UserWorkModel : UserBaseModel
    {
        [Display(Name = "Old password")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

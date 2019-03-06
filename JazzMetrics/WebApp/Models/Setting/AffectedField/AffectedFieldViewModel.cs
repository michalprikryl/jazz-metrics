using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Setting.AffectedField
{
    public class AffectedFieldListModel : ViewModel
    {
        public List<AffectedFieldViewModel> AffectedFields { get; set; }
    }

    public class AffectedFieldWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
    }

    public class AffectedFieldViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

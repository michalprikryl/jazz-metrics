using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Setting.AspiceProcess
{
    public class AspiceProcessListModel : ViewModel
    {
        public List<AspiceProcessViewModel> AspiceProcesses { get; set; }
    }

    public class AspiceProcessWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Shortcut")]
        [Required(ErrorMessage = "Shortcut is required!")]
        public string Shortcut { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        [Display(Name = "Automotive SPICE version")]
        [Required(ErrorMessage = "Choose Automotive SPICE version, please!")]
        public string AspiceVersion { get; set; }
        public List<SelectListItem> AspiceVersions { get; set; }
    }

    public class AspiceProcessViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public int AspiceVersionId { get; set; }
        public string AspiceVersion { get; set; }
    }
}

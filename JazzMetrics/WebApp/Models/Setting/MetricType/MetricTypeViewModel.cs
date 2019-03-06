using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Setting.MetricType
{
    public class MetricTypeListModel : ViewModel
    {
        public List<MetricTypeViewModel> MetricTypes { get; set; }
    }

    public class MetricTypeWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
    }

    public class MetricTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

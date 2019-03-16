using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Setting.Metric
{
    public class MetricListModel : ViewModel
    {
        public List<MetricViewModel> Metrics { get; set; }
    }

    public class MetricWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Identificator")]
        [Required(ErrorMessage = "Identificator is required!")]
        public string Identificator { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        [Display(Name = "Metric type")]
        [Required(ErrorMessage = "Choose metric type, please!")]
        public string MetricTypeId { get; set; }
        public List<SelectListItem> MetricTypes { get; set; }

        [Display(Name = "Automotive SPICE process")]
        [Required(ErrorMessage = "Choose Automotive SPICE process, please!")]
        public string AspiceProcessId { get; set; }
        public List<SelectListItem> AspiceProcesses { get; set; }

        [Display(Name = "Affected field")]
        [Required(ErrorMessage = "Choose metric affected field, please!")]
        public string AffectedFieldId { get; set; }
        public List<SelectListItem> AffectedFields { get; set; }

        [Display(Name = "Public metric (users from any other company can use this metric)")]
        public bool Public { get; set; }

        public List<MetricColumn> NumberColumns { get; set; }
        public List<MetricCoverageColumn> CoverageColumns { get; set; }

        public MetricWorkModel()
        {
            NumberColumns = new List<MetricColumn>();
            CoverageColumns = new List<MetricCoverageColumn>();
        }
    }

    public class MetricViewModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MetricTypeId { get; set; }
        public string MetricType { get; set; }
        public int AspiceProcessId { get; set; }
        public string AspiceProcess { get; set; }
        public int AffectedFieldId { get; set; }
        public string AffectedField { get; set; }
        public bool Public { get; set; }
    }

    public class NewMetricColumn
    {
        public int Index { get; set; }
        public string Type { get; set; }
    }

    public class MetricColumn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        [Display(Name = "Attribute name")]
        public string Name1 { get; set; }

        public bool Deleted { get; set; }
    }

    public class MetricCoverageColumn : MetricColumn
    {
        public int Id2 { get; set; }

        [Display(Name = "Attribute name")]
        public string Name2 { get; set; }
    }
}

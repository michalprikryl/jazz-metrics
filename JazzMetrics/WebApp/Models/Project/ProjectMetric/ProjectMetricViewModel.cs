using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Project.ProjectMetric
{
    public class ProjectMetricListModel : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProjectMetricViewModel> Metrics { get; set; }
    }

    public class ProjectMetricWorkModel : ViewModel
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        [Display(Name = "URL for data retrieval")]
        [Required(ErrorMessage = "Paste URL, please!")]
        public string DataUrl { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Paste your username, please!")]
        public string DataUsername { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string DataPassword { get; set; }

        [Display(Name = "I want to be warn, when value is lower than number below (available only for coverage metrics).")]
        public bool Warning { get; set; }

        [Display(Name = "Minimal value for warning")]
        [Range(0, int.MaxValue, ErrorMessage = "Minimal value must be greater than 0!")]
        public decimal? MinimalWarningValue { get; set; }

        [Display(Name = "Metric")]
        public string MetricId { get; set; }
        public List<SelectListItem> Metrics { get; set; }
    }

    public class ProjectMetricViewModel
    {
        public int MetricId { get; set; }
        public int ProjectMetricId { get; set; }
        public string MetricInfo { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool Warning { get; set; }
        public bool Public { get; set; }
        public bool CanEdit { get; set; }
    }
}

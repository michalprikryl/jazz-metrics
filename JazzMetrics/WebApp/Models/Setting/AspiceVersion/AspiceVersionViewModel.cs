﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Setting.AspiceVersion
{
    public class AspiceVersionListModel : ViewModel
    {
        public List<AspiceVersionViewModel> AspiceVersions { get; set; }
    }

    public class AspiceVersionWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Version number")]
        [Required(ErrorMessage = "Version number is required!")]
        [Range(0, int.MaxValue, ErrorMessage = "Version must be greater than 0!")]
        [DisplayFormat(DataFormatString = "{0:0.0}", ApplyFormatInEditMode = true)]
        public decimal VersionNumber { get; set; }

        [Display(Name = "Release date")]
        [Required(ErrorMessage = "Release date is required!")]
        public string ReleaseDate { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }
    }

    public class AspiceVersionViewModel
    {
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
    }
}

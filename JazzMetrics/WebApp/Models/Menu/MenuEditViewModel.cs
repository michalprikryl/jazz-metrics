﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.Models.Menu
{
    public class MenuEditViewModel
    {
        [Required]
        [Display(Name = "Název")]
        public string Name { get; set; }
        [Display(Name = "Název volané funkce API")]
        public string FunctionID { get; set; }
        [Display(Name = "Parametry volané funkce")]
        public string Parameters { get; set; }
        [Required]
        [Display(Name = "Viditelné")]
        public bool Visible { get; set; }
        public int ID { get; set; }
        public bool Last { get; set; }
    }
}
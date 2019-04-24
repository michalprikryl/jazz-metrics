using Library.Models.ProjectMetrics;
using Library.Models.ProjectUsers;
using System;
using System.Collections.Generic;

namespace Library.Models.Projects
{
    /// <summary>
    /// model predstavujici projekt
    /// </summary>
    public class ProjectModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// nazev projektu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// popis projektu
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// datum a cas vytvoreni projektu
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// seznam projektovych metrik
        /// </summary>
        public List<ProjectMetricModel> ProjectMetrics { get; set; }
        /// <summary>
        /// seznam projektovych uzivatelu
        /// </summary>
        public List<ProjectUserModel> ProjectUsers { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
    }
}

using Library.Models.Metric;
using Library.Models.ProjectMetricSnapshots;
using Library.Models.Projects;
using System;
using System.Collections.Generic;

namespace Library.Models.ProjectMetrics
{
    /// <summary>
    /// model predstavujici projektovou metriku
    /// </summary>
    public class ProjectMetricModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ID projektu
        /// </summary>
        public int ProjectId { get; set; }
        /// <summary>
        /// ID metriky
        /// </summary>
        public int MetricId { get; set; }
        /// <summary>
        /// datum a cas vytvoreni projektove metriky
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// datum a cas posledni updatu dat projektove metriky
        /// </summary>
        public DateTime LastUpdateDate { get; set; }
        /// <summary>
        /// URL adresa, ze ktere se ziskavaji data
        /// </summary>
        public string DataUrl { get; set; }
        /// <summary>
        /// prihlasovaci jmeno na URL adresu (momentalne HTTP Basic autentizace)
        /// </summary>
        public string DataUsername { get; set; }
        /// <summary>
        /// heslo na URL adresu (momentalne HTTP Basic autentizace)
        /// </summary>
        public string DataPassword { get; set; }
        /// <summary>
        /// true -> pokud je to metrika pokryti, bude hlidana nizka hodnota a klesajici trend
        /// </summary>
        public bool Warning { get; set; }
        /// <summary>
        /// prahova hodnota, pokud je warning == true, tak se hlida aby hodnota metriky neklesla pod tuto hodnotu
        /// </summary>
        public decimal? MinimalWarningValue { get; set; }

        /// <summary>
        /// metrika
        /// </summary>
        public MetricModel Metric { get; set; }
        /// <summary>
        /// projekt
        /// </summary>
        public ProjectModel Project { get; set; }

        /// <summary>
        /// snimky metriky -> informace u updatu dat
        /// </summary>
        public List<ProjectMetricSnapshotModel> Snapshots { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(DataUrl) && !string.IsNullOrEmpty(DataUsername) && (DataPassword != null || DataPassword != string.Empty);
    }
}

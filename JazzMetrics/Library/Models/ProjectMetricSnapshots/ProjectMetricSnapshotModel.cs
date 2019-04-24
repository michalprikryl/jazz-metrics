using System;
using System.Collections.Generic;

namespace Library.Models.ProjectMetricSnapshots
{
    /// <summary>
    /// model predstavujici snimek metriky
    /// </summary>
    public class ProjectMetricSnapshotModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// datum a cas vlozeni/vytvoreni
        /// </summary>
        public DateTime InsertionDate { get; set; }
        /// <summary>
        /// ID projektove metriky
        /// </summary>
        public int ProjectMetricId { get; set; }

        /// <summary>
        /// hodnoty snimku dat pro jednotlive sloupce metriky
        /// </summary>
        public List<ProjectMetricColumnValueModel> Values { get; set; }
    }
}

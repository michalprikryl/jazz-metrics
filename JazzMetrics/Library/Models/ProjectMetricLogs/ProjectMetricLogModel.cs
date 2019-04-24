using System;

namespace Library.Models.ProjectMetricLogs
{
    /// <summary>
    /// model predstavujici zpravu o projektove metrice
    /// </summary>
    public class ProjectMetricLogModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// zprava
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// true -> jedna se o chybu
        /// </summary>
        public bool Warning { get; set; }
        /// <summary>
        /// ID projektove metriky
        /// </summary>
        public int ProjectMetricId { get; set; }
        /// <summary>
        /// datum a cas vytvoreni zpravy
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}

namespace Library.Models.ProjectMetricSnapshots
{
    /// <summary>
    /// model reprezentujici ziskanou hodnotu sloupce metriky pro projekt
    /// </summary>
    public class ProjectMetricColumnValueModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// samotna ciselna hodnota
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// ID snimku, ke kteremu patri
        /// </summary>
        public int ProjectMetricSnapshotId { get; set; }
        /// <summary>
        /// ID atributu (sloupce) metriky
        /// </summary>
        public int MetricColumnId { get; set; }
    }
}

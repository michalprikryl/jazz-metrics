using Library;
using Library.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Models.Project.Dashboard
{
    public class ProjectDashboardViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// metriky projektu
        /// </summary>
        public List<MetricDataModel> Metrics { get; set; }

        public void FillMetrics(ProjectModel model)
        {
            Metrics = new List<MetricDataModel>();
            foreach (var projectMetric in model.ProjectMetrics)
            {
                MetricDataModel metric = new MetricDataModel
                {
                    Id = projectMetric.Id,
                    Warning = false,
                    MetricName = projectMetric.Metric.Name,
                    MetricIdentificator = projectMetric.Metric.Identificator,
                    MetricDescription = projectMetric.Metric.Description,
                    MetricColumns = new List<MetricColumnModel>()
                };

                if (projectMetric.Metric.MetricType.CoverageMetric)
                {
                    foreach (var column in projectMetric.Metric.Columns)
                    {
                        var snapshots = projectMetric.Snapshots.Where(s => s.Values.Any(v => v.MetricColumnId == column.Id))
                            .Select(s => new
                            {
                                date = s.InsertionDate.GetDateTimeString(),
                                values = s.Values.Where(v => v.MetricColumnId == column.Id).Select(v => v.Value).ToList()
                            });

                        metric.MetricColumns.Add(
                            new MetricColumnModel
                            {
                                Type = ChartType.Line,
                                Titles = new List<string> { column.CoverageName },
                                Labels = snapshots.Select(s => s.date).ToList(),
                                Values = new List<List<decimal>> { snapshots.SelectMany(s => s.values).ToList() }
                            });
                    }

                    if (projectMetric.Warning)
                    {
                        metric.Warning = metric.MetricColumns.Any(c => c.Values.First().Any() && c.Values.First().Last() <= projectMetric.MinimalWarningValue);
                        metric.DecreasingTrendWarning = metric.MetricColumns.Any(c =>
                        {
                            var values = c.Values.First();
                            if (values.Count > 2)
                            {
                                var lastThreeValues = values.Skip(Math.Max(values.Count - 3, 0)).ToArray();
                                for (int i = 1; i < lastThreeValues.Length; i++)
                                {
                                    if (lastThreeValues[i] >= lastThreeValues[i - 1])
                                    {
                                        return false;
                                    }
                                }

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        });
                    }
                }
                else if (projectMetric.Metric.MetricType.NumberMetric)
                {
                    MetricColumnModel columnModel = new MetricColumnModel
                    {
                        Type = ChartType.Bar,
                        Titles = new List<string>(),
                        Values = new List<List<decimal>>(),
                        Labels = projectMetric.Metric.Columns.Select(c => string.IsNullOrEmpty(c.Value) ? "no value" : c.Value).ToList()
                    };

                    foreach (var snapshot in projectMetric.Snapshots)
                    {
                        columnModel.Titles.Add(snapshot.InsertionDate.GetDateTimeString());
                        columnModel.Values.Add(snapshot.Values.Select(v => v.Value).ToList());
                    }

                    metric.MetricColumns.Add(columnModel);
                }
                else
                {
                    MetricColumnModel columnModel = new MetricColumnModel
                    {
                        Type = ChartType.Unknown,
                        Labels = new List<string>(),
                        Titles = new List<string>(),
                        Values = new List<List<decimal>>(),
                    };

                    metric.MetricColumns.Add(columnModel);
                }

                Metrics.Add(metric);
            }
        }

        public void FillMetricsWithTestValues()
        {
            MetricColumnModel columnCoverage = new MetricColumnModel
            {
                Type = ChartType.Line,
                Labels = new List<string>
                {
                    "2019-01-08 09:30:26.123",
                    "2019-01-08 09:35:26.123",
                    "2019-01-08 10:30:26.123",
                    "2019-01-09 06:21:26.123"
                },
                Titles = new List<string> { "" },
                Values = new List<List<decimal>>
                {
                    new List<decimal>{ 4, 15, 99, 55 }
                }
            };

            MetricColumnModel numberColumn = new MetricColumnModel
            {
                Type = ChartType.Bar,
                Labels = new List<string>
                {
                    "Under construction",
                    "Ready to review",
                    "Reviewed",
                    "Implemented",
                    "Tested",
                    "No status"
                },
                Titles = new List<string>
                {
                    DateTime.Now.AddDays(-2).GetDateTimeStringLong(),
                    DateTime.Now.AddDays(-1).GetDateTimeStringLong(),
                    DateTime.Now.AddDays(0).GetDateTimeStringLong()
                },
                Values = new List<List<decimal>>
                {
                    new List<decimal>{ 20, 10, 15, 11, 5, 0 },
                    new List<decimal>{ 15, 5, 10, 6, 0, 5 },
                    new List<decimal>{ 10, 2, 5, 3, 0, 10 }
                }
            };

            MetricDataModel metric = new MetricDataModel
            {
                MetricIdentificator = "M01",
                DecreasingTrendWarning = true,
                MetricName = "very useful metric",
                MetricDescription = "Description about very useful metric",
                MetricColumns = new List<MetricColumnModel>
                {
                    columnCoverage, numberColumn
                }
            };

            MetricColumnModel columnCoverage2 = new MetricColumnModel
            {
                Type = ChartType.Line,
                Labels = new List<string>
                {
                    "2019-01-08 09:30:26.123",
                    "2019-01-08 09:35:26.123",
                    "2019-01-08 10:30:26.123",
                    "2019-01-08 11:45:26.123",
                    "2019-01-08 12:51:26.123",
                    "2019-01-08 13:25:26.123",
                    "2019-01-09 05:35:26.123",
                    "2019-01-09 06:21:26.123"
                },
                Titles = new List<string> { "" },
                Values = new List<List<decimal>>
                {
                    new List<decimal>{ 40, 5, 65, 55, 88, 44, 36, 77 }
                }
            };

            MetricDataModel metric2 = new MetricDataModel
            {
                Warning = true,
                MetricIdentificator = "M02",
                MetricName = "other useful metric",
                MetricDescription = "Description about other useful metric",
                MetricColumns = new List<MetricColumnModel>
                {
                    columnCoverage2
                }
            };

            Metrics = new List<MetricDataModel> { metric, metric2 };
        }
    }

    public class MetricDataModel
    {
        public int Id { get; set; }
        public bool Warning { get; set; }
        public bool DecreasingTrendWarning { get; set; }
        public string MetricIdentificator { get; set; }
        public string MetricName { get; set; }
        public string MetricInfo { get => $"{MetricIdentificator} - {MetricName}"; }
        public string MetricDescription { get; set; }
        /// <summary>
        /// data metriky pro zobrazeni vsech grafu
        /// </summary>
        public List<MetricColumnModel> MetricColumns { get; set; }
    }

    /// <summary>
    /// trida pro uchovani dat metriky pro zobrazeni vsech jejich grafu
    /// </summary>
    public class MetricColumnModel
    {
        /// <summary>
        /// typ grafu, momentalne jsou dva
        /// </summary>
        public ChartType Type { get; set; }
        /// <summary>
        /// hodnoty grafu (osa X)
        /// </summary>
        public List<string> Labels { get; set; }
        /// <summary>
        /// titulek nad graf - pro coverage jeden, pro number jich bude vice
        /// </summary>
        public List<string> Titles { get; set; }
        /// <summary>
        /// hodnoty grafu (osa Y) - pro coverage jeden, pro number jich bude vice
        /// </summary>
        public List<List<decimal>> Values { get; set; }
    }

    /// <summary>
    /// trida pro generovani canvasu s grafem
    /// </summary>
    public class ChartModel
    {
        public string ChartId { get; set; }
        public string MetricInfo { get; set; }
    }

    /// <summary>
    /// podporovane druhy grafu
    /// </summary>
    public enum ChartType
    {
        /// <summary>
        /// pro number
        /// </summary>
        Bar,
        /// <summary>
        /// pro coverage
        /// </summary>
        Line,
        /// <summary>
        /// neznamy
        /// </summary>
        Unknown
    }

    public class ExportViewModel
    {
        public List<MetricExportViewModel> Metrics { get; set; }
    }

    public class MetricExportViewModel
    {
        public string MetricName { get; set; }
        public List<string> Charts { get; set; }
    }
}

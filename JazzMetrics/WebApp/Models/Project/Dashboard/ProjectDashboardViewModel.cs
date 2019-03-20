﻿using Library;
using System;
using System.Collections.Generic;

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
                MetricInfo = "M01 - very useful metric",
                MetricDescription = "Description about very useful metric",
                MetricColumns = new List<MetricColumnModel>
                {
                    columnCoverage, numberColumn
                }
            };

            Metrics = new List<MetricDataModel> { metric };
        }
    }

    public class MetricDataModel
    {
        public string MetricInfo { get; set; }
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
        Line
    }
}

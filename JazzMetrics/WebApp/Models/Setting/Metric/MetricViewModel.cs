﻿using Library.Models.Metric;
using Library.Models.MetricColumn;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WebApp.Models.Setting.Metric
{
    public class MetricListModel : ViewModel
    {
        public List<MetricViewModel> Metrics { get; set; }
    }

    public class MetricWorkModel : ViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Identificator")]
        [Required(ErrorMessage = "Identificator is required!")]
        public string Identificator { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required!")]
        public string Description { get; set; }

        [Display(Name = "Requirement group")]
        [Required(ErrorMessage = "Requirement group is required!")]
        public string RequirementGroup { get; set; }

        [Display(Name = "Metric type")]
        public string MetricTypeId { get; set; }
        public List<SelectListItem> MetricTypes { get; set; }

        [Display(Name = "Automotive SPICE process")]
        [Required(ErrorMessage = "Choose Automotive SPICE process, please!")]
        public string AspiceProcessId { get; set; }
        public List<SelectListItem> AspiceProcesses { get; set; }

        [Display(Name = "Affected field")]
        [Required(ErrorMessage = "Choose metric affected field, please!")]
        public string AffectedFieldId { get; set; }
        public List<SelectListItem> AffectedFields { get; set; }

        [Display(Name = "Public metric (users from any other companies can use this metric)")]
        public bool Public { get; set; }

        public List<MetricColumn> NumberColumns { get; set; }
        public List<MetricCoverageColumn> CoverageColumns { get; set; }

        public MetricWorkModel()
        {
            NumberColumns = new List<MetricColumn>();
            CoverageColumns = new List<MetricCoverageColumn>();
        }

        public void DropDeletedColumns()
        {
            NumberColumns.RemoveAll(m => m.Deleted);
            CoverageColumns.RemoveAll(m => m.Deleted);
        }

        public MetricModel TranslateToMetricModel()
        {
            MetricModel model = GetMetricModel();

            if (NumberColumns.Count > 0)
            {
                model.Columns.AddRange(NumberColumns.Select(n =>
                    new MetricColumnModel
                    {
                        Id = n.Id,
                        Value = n.Value,
                        FieldName = n.FieldName,
                        NumberFieldName = n.NumberFieldName
                    }));
            }
            else
            {
                model.Columns.AddRange(CoverageColumns.Select(n =>
                    new MetricColumnModel
                    {
                        Id = n.Id,
                        Value = n.Value,
                        FieldName = n.FieldName,
                        DivisorValue = n.DivisorValue,
                        DivisorFieldName = n.DivisorFieldName,
                        CoverageName = n.CoverageName
                    }));
            }

            return model;
        }

        public void LoadMetricColumns(List<MetricColumnModel> columns)
        {
            int i = 0;
            foreach (var item in columns.Where(c => string.IsNullOrEmpty(c.DivisorValue)))
            {
                NumberColumns.Add(new MetricColumn
                {
                    Id = item.Id,
                    Index = i++,
                    Value = item.Value,
                    FieldName = item.FieldName,
                    NumberFieldName = item.NumberFieldName,
                    Deleted = false
                });
            }

            i = 0;
            foreach (var item in columns.Where(c => !string.IsNullOrEmpty(c.DivisorValue)))
            {
                CoverageColumns.Add(new MetricCoverageColumn
                {
                    Id = item.Id,
                    Index = i++,
                    Value = item.Value,
                    FieldName = item.FieldName,
                    DivisorValue = item.DivisorValue,
                    DivisorFieldName = item.DivisorFieldName,
                    CoverageName = item.CoverageName,
                    Deleted = false
                });
            }
        }

        private MetricModel GetMetricModel() =>
            new MetricModel
            {
                Id = Id,
                Identificator = Identificator,
                Name = Name,
                Description = Description,
                RequirementGroup = RequirementGroup,
                MetricTypeId = int.Parse(MetricTypeId),
                AspiceProcessId = int.Parse(AspiceProcessId),
                AffectedFieldId = int.Parse(AffectedFieldId),
                Public = Public,
                Columns = new List<MetricColumnModel>()
            };
    }

    public class MetricViewModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MetricTypeId { get; set; }
        public string MetricType { get; set; }
        public int AspiceProcessId { get; set; }
        public string AspiceProcess { get; set; }
        public int AffectedFieldId { get; set; }
        public string AffectedField { get; set; }
        public bool Public { get; set; }
        public int? CompanyId { get; set; }
    }

    public class NewMetricColumn
    {
        public int Index { get; set; }
        public string Type { get; set; }
    }

    public class MetricColumn
    {
        public int Id { get; set; }

        public int Index { get; set; }

        [Display(Name = "Column attributes")]
        public string Value { get; set; }

        public string FieldName { get; set; }

        public string NumberFieldName { get; set; }

        public bool Deleted { get; set; }
    }

    public class MetricCoverageColumn : MetricColumn
    {
        [Display(Name = "Coverage attributes")]
        public string DivisorValue { get; set; }

        public string DivisorFieldName { get; set; }

        public string CoverageName { get; set; }
    }

    public class MetricDetailViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RequirementGroup { get; set; }
        public string MetricType { get; set; }
        public string AspiceProcess { get; set; }
        public string AffectedField { get; set; }
        public bool Public { get; set; }
        public int? CompanyId { get; set; }
        public List<string> Columns { get; set; }

        public void LoadMetricColumns(List<MetricColumnModel> columns)
        {
            Columns = columns.Where(c => string.IsNullOrEmpty(c.DivisorValue)).Select(c => $"{GetValue(c.Value)} (XML tag - {c.FieldName}), XML tag with number - {c.NumberFieldName}").ToList();

            foreach (var item in columns.Where(c => !string.IsNullOrEmpty(c.DivisorValue)))
            {
                Columns.Add($"{item.CoverageName} -> '{GetValue(item.Value)}' (XML tag - {item.FieldName})  divided by '{item.DivisorValue}' (XML tag - {item.DivisorFieldName})");
            }
        }

        private string GetValue(string value) => value == string.Empty ? "no value" : $"'{value}'";
    }
}

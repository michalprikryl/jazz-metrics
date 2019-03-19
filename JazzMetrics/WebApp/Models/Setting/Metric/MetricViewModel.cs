using Library.Models.Metric;
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

        [Display(Name = "Metric type")]
        [Required(ErrorMessage = "Choose metric type, please!")]
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

        [Display(Name = "Public metric (users from any other company can use this metric)")]
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

            model.Columns.AddRange(NumberColumns.Select(n =>
                new MetricColumnModel
                {
                    Id = n.Id,
                    Name = n.Name1
                }));

            foreach (var item in CoverageColumns)
            {
                model.Columns.Add(new MetricColumnModel { Id = item.Id, Name = item.Name1, PairMetricColumnId = 0 });
                model.Columns.Add(new MetricColumnModel { Id = item.Id2, Name = item.Name2, PairMetricColumnId = 0 });
            }

            return model;
        }

        public void LoadMetricColumns(List<MetricColumnModel> columns)
        {
            int i = 0;
            foreach (var item in columns.Where(c => !c.Divisor.HasValue))
            {
                NumberColumns.Add(new MetricColumn
                {
                    Id = item.Id,
                    Index = i++,
                    Name1 = item.Name,
                    Deleted = false
                });
            }

            i = 0;
            foreach (var item in columns.Where(c => c.PairMetricColumnId.HasValue))
            {
                var secondColumn = columns.First(c => c.Id == item.PairMetricColumnId.Value);
                CoverageColumns.Add(new MetricCoverageColumn
                {
                    Id = item.Id,
                    Id2 = secondColumn.Id,
                    Index = i++,
                    Name1 = item.Name,
                    Name2 = secondColumn.Name,
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

        [Display(Name = "Attribute name")]
        public string Name1 { get; set; }

        public bool Deleted { get; set; }
    }

    public class MetricCoverageColumn : MetricColumn
    {
        public int Id2 { get; set; }

        [Display(Name = "Attribute 1 and 2 names")]
        public string Name2 { get; set; }
    }

    public class MetricDetailViewModel : ViewModel
    {
        public int Id { get; set; }
        public string Identificator { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MetricType { get; set; }
        public string AspiceProcess { get; set; }
        public string AffectedField { get; set; }
        public bool Public { get; set; }
        public int? CompanyId { get; set; }
        public List<string> Columns { get; set; }

        public void LoadMetricColumns(List<MetricColumnModel> columns)
        {
            Columns = columns.Where(c => !c.Divisor.HasValue).Select(c => $"'{c.Name}' (type - number)").ToList();

            foreach (var item in columns.Where(c => c.PairMetricColumnId.HasValue))
            {
                var secondColumn = columns.First(c => c.Id == item.PairMetricColumnId.Value);
                Columns.Add($"'{item.Name}' divided by '{secondColumn.Name}' (type - coverage)");
            }
        }
    }
}

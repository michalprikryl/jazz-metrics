using Library.Models.AffectedFields;
using Library.Models.AspiceProcesses;
using Library.Models.MetricColumn;
using Library.Models.MetricType;
using System.Collections.Generic;

namespace Library.Models.Metric
{
    /// <summary>
    /// model reprezentujici metriku
    /// </summary>
    public class MetricModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// jedinecny identifikator metriky
        /// </summary>
        public string Identificator { get; set; }
        /// <summary>
        /// nazev metriky
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// popis metriky
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// skupina metriky
        /// </summary>
        public string RequirementGroup { get; set; }
        /// <summary>
        /// ID typu metriky
        /// </summary>
        public int MetricTypeId { get; set; }
        /// <summary>
        /// ID procesu ASPICE
        /// </summary>
        public int AspiceProcessId { get; set; }
        /// <summary>
        /// ID ovlivnene oblasti QA
        /// </summary>
        public int AffectedFieldId { get; set; }
        /// <summary>
        /// true -> metrika je dostupna vsem firmam v nastroji
        /// </summary>
        public bool Public { get; set; }
        /// <summary>
        /// ID spolecnosti, ktera muze metriku spravovat
        /// </summary>
        public int? CompanyId { get; set; }

        /// <summary>
        /// typ metriky
        /// </summary>
        public MetricTypeModel MetricType { get; set; }
        /// <summary>
        /// ovlivnena oblast QA
        /// </summary>
        public AffectedFieldModel AffectedField { get; set; }
        /// <summary>
        /// proces ASPICE
        /// </summary>
        public AspiceProcessModel AspiceProcess { get; set; }

        /// <summary>
        /// seznam atributu (sloupcu) metriky
        /// </summary>
        public List<MetricColumnModel> Columns { get; set; }

        /// <summary>
        /// kontrola, zda jsou vyplnene povinne parametry
        /// </summary>
        /// <returns></returns>
        public bool Validate() => !string.IsNullOrEmpty(Identificator) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description) && !string.IsNullOrEmpty(RequirementGroup);

        /// <summary>
        /// reprezentace metriky ve forme retezce
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Name} ({Identificator})";
    }
}

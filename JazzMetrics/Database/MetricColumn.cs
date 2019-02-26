//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class MetricColumn
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MetricColumn()
        {
            this.MetricColumn1 = new HashSet<MetricColumn>();
            this.ProjectMetricColumnValues = new HashSet<ProjectMetricColumnValue>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public int MetricID { get; set; }
        public Nullable<int> PairMetricColumnID { get; set; }
    
        public virtual Metric Metric { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MetricColumn> MetricColumn1 { get; set; }
        public virtual MetricColumn MetricColumn2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectMetricColumnValue> ProjectMetricColumnValues { get; set; }
    }
}

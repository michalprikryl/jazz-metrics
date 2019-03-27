namespace Library.Models.MetricColumn
{
    public class MetricColumnModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string FieldName { get; set; }
        public string NumberFieldName { get; set; }
        public string DivisorValue { get; set; }
        public string DivisorFieldName { get; set; }
        public int MetricId { get; set; }

        public bool Validate() =>
            (string.IsNullOrEmpty(DivisorFieldName) && !string.IsNullOrEmpty(FieldName) && !string.IsNullOrEmpty(NumberFieldName)) //number column
            || (!string.IsNullOrEmpty(DivisorFieldName) && !string.IsNullOrEmpty(FieldName)); //coverage column
    }
}

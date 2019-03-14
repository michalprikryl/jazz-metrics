namespace Library.Models.MetricType
{
    public class MetricTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);

        public override string ToString() => $"{Name}";
    }
}

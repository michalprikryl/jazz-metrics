namespace WebApp.Models.Setting.MetricType
{
    public class MetricTypeModel : BaseApiResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

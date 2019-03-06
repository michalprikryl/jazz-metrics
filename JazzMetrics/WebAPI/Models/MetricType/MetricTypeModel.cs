namespace WebAPI.Models.MetricType
{
    public class MetricTypeModel : BaseResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
        }
    }
}

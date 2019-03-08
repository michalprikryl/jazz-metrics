using WebAPI.Models.AspiceVersions;

namespace WebAPI.Models.AspiceProcesses
{
    public class AspiceProcessModel : BaseResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public int AspiceVersionId { get; set; }
        public AspiceVersionModel AspiceVersion { get; set; }

        public bool Validate
        {
            get => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Shortcut) && !string.IsNullOrEmpty(Description);
        }
    }
}

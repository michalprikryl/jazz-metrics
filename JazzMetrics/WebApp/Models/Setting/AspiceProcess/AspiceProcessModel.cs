using WebApp.Models.Setting.AspiceVersion;

namespace WebApp.Models.Setting.AspiceProcess
{
    public class AspiceProcessModel : BaseApiResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public int AspiceVersionId { get; set; }
        public AspiceVersionModel AspiceVersion { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Shortcut})";
        }
    }
}

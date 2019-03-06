using System;

namespace WebApp.Models.Setting.AspiceVersion
{
    public class AspiceVersionModel : BaseApiResult
    {
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
    }
}

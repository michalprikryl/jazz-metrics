using System;

namespace WebAPI.Models.AspiceVersions
{
    public class AspiceVersionModel : BaseResponseModel
    {
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }

        public bool Validate
        {
            get => VersionNumber > 0 && ReleaseDate > DateTime.Now.AddYears(-20) && !string.IsNullOrEmpty(Description);
        }
    }
}

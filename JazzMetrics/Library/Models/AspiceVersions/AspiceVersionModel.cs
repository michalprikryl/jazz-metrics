using System;

namespace Library.Models.AspiceVersions
{
    public class AspiceVersionModel
    {
        public int Id { get; set; }
        public decimal VersionNumber { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }

        public bool Validate() => VersionNumber > 0 && ReleaseDate > DateTime.Now.AddYears(-20) && !string.IsNullOrEmpty(Description);

        public override string ToString() => $"Version {VersionNumber}";
    }
}

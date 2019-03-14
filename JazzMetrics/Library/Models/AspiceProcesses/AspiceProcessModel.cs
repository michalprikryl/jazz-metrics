using Library.Models.AspiceVersions;

namespace Library.Models.AspiceProcesses
{
    public class AspiceProcessModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Description { get; set; }
        public int AspiceVersionId { get; set; }
        public AspiceVersionModel AspiceVersion { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Shortcut) && !string.IsNullOrEmpty(Description);

        public override string ToString() => $"{Name} ({Shortcut})";
    }
}

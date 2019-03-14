namespace Library.Models.AffectedFields
{
    public class AffectedFieldModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Validate() => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}

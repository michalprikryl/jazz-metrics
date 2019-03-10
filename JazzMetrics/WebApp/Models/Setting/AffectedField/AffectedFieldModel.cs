namespace WebApp.Models.Setting.AffectedField
{
    public class AffectedFieldModel : BaseApiResult
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

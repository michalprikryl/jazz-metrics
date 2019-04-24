namespace Library.Models
{
    public class CurrentUser
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string RoleName { get; set; }
    }
}

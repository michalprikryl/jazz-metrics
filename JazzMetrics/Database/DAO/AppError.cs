using System;

namespace Database.DAO
{
    public partial class AppError
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string Module { get; set; }
        public string Function { get; set; }
        public string Exception { get; set; }
        public string InnerException { get; set; }
        public string Message { get; set; }
        public bool Solved { get; set; }
        public bool Deleted { get; set; }
        public string AppInfo { get; set; }
    }
}

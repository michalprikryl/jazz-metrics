using System;
using System.Collections.Generic;

namespace WebApp.Models.Setting.AppError
{
    public class AppErrorListModel : ViewModel
    {
        public List<AppErrorViewModel> AppErrors { get; set; }
    }

    public class AppErrorViewModel
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

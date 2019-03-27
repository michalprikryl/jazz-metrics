using System;

namespace Database.DAO
{
    public partial class ProjectMetricLog
    {
        public ProjectMetricLog(string message, bool warning = true)
        {
            Message = message;
            Warning = warning;
            CreateDate = DateTime.Now;
        }
    }
}

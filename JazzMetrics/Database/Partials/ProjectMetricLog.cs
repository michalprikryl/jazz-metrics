using System;

namespace Database.DAO
{
    public partial class ProjectMetricLog
    {
        /// <summary>
        /// log metriky
        /// </summary>
        /// <param name="message">zprava</param>
        /// <param name="warning">true - je to warning a naopak</param>
        public ProjectMetricLog(string message, bool warning = true)
        {
            Message = message;
            Warning = warning;
            CreateDate = DateTime.Now;
        }
    }
}

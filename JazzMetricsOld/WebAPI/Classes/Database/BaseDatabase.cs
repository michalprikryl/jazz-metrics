using Database;
using Newtonsoft.Json;

namespace WebAPI.Classes.Database
{
    /// <summary>
    /// bazova trida poskytujici pristup k nove DB
    /// </summary>
    public class BaseDatabase
    {
        /// <summary>
        /// databazovy kontext pro novou databazi
        /// </summary>
        [JsonIgnore]
        public JazzMetricsEntities DatabaseContext { get; protected set; }
    }
}
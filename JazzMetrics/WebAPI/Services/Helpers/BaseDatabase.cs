using Database;

namespace WebAPI.Services.Helpers
{
    /// <summary>
    /// bazova trida poskytujici pristup k DB
    /// </summary>
    public class BaseDatabase
    {
        /// <summary>
        /// pristup k databazi
        /// </summary>
        protected readonly JazzMetricsContext Database;

        /// <summary>
        /// konstruktor, ktery ulozi databazovy kontext do property
        /// </summary>
        /// <param name="db">DB kontext</param>
        public BaseDatabase(JazzMetricsContext db)
        {
            Database = db;
        }
    }
}

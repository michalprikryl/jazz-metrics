using Library.Models.Test;
using WebApp.Models.Partials;

namespace WebApp.Models.Test
{
    /// <summary>
    /// kompletni vysledek testu pripojeni - obsahuje i info o samotnem pripojeni na API
    /// </summary>
    public class TestViewModel : TestModel
    {
        /// <summary>
        /// zda je dostupne pripojeni k API
        /// </summary>
        public bool ConnectionApi { get; set; }
        /// <summary>
        /// zprava o pripojeni k API
        /// </summary>
        public string MessageApi { get; set; }
        /// <summary>
        /// ciselny kod HTTP odpovedi z API
        /// </summary>
        public int HTTPResponseApi { get; set; }
        /// <summary>
        /// message k ikone o API
        /// </summary>
        public ResultViewModel ApiResultMessage { get; set; }
        /// <summary>
        /// message k ikone o DB
        /// </summary>
        public ResultViewModel DbResultMessage { get; set; }
    }
}

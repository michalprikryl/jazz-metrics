namespace WebApp.Classes.Test
{
    /// <summary>
    /// vysledek testu pripojeni k DB ziskany z API
    /// </summary>
    public class TestResultAPI
    {
        /// <summary>
        /// zda je dostupne pripojeni k DB
        /// </summary>
        public bool ConnectionDB { get; set; }
        /// <summary>
        /// zprava o pripojeni k DB
        /// </summary>
        public string MessageDB { get; set; }
    }

    /// <summary>
    /// kompletni vysledek testu pripojeni - obsahuje i info o samotnem pripojeni na API
    /// </summary>
    public class TestResult : TestResultAPI
    {
        /// <summary>
        /// zda je dostupne pripojeni k API
        /// </summary>
        public bool ConnectionAPI { get; set; }
        /// <summary>
        /// zprava o pripojeni k API
        /// </summary>
        public string MessageAPI { get; set; }
        /// <summary>
        /// ciselny kod HTTP odpovedi z API
        /// </summary>
        public int HTTPResponseAPI { get; set; }
    }
}
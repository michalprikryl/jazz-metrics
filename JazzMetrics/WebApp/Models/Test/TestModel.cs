namespace WebApp.Models.Test
{
    /// <summary>
    /// vysledek testu pripojeni k DB ziskany z API
    /// </summary>
    public class TestModel
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
}

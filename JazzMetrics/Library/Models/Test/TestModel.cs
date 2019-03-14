namespace Library.Models.Test
{
    /// <summary>
    /// trida reprezentujici vysledek testu pripojeni na DB
    /// </summary>
    public class TestModel
    {
        /// <summary>
        /// jde pripojeni, jonebone?
        /// </summary>
        public bool ConnectionDB { get; set; }
        /// <summary>
        /// pripadna exception message
        /// </summary>
        public string MessageDB { get; set; }
        /// <summary>
        /// verze API
        /// </summary>
        public string ApiVersion { get; set; }
    }
}

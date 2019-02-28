using System;

namespace WebApp.Models.Error
{
    /// <summary>
    /// trida, reprezentujici error zasilany naa API
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// cas vzniku erroru
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// modul, kde nastala chyba
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// funkce, ve ktere nastala chyba
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// chybova zprava z exception
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// chybova zprava z inner exception naseho exception
        /// </summary>
        public string InnerExceptionMessage { get; set; }
        /// <summary>
        /// zprava programatora
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// ID uzivatele, pod kterym nastala chyba
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// typ exception (ApplicationException, IndexOutOfRangeException etc.)
        /// </summary>
        public string ExceptionType { get; set; }

        public ErrorModel() { }

        /// <summary>
        /// inicializace objektu
        /// </summary>
        /// <param name="e">objekt nastale vyjimky</param>
        /// <param name="userID">ID uzivatele, pod kterym nastala chyba</param>
        /// <param name="message">zprava programatora</param>
        /// <param name="module">modul, kde nastala chyba</param>
        /// <param name="function">funkce, ve ktere nastala chyba</param>
        public ErrorModel(Exception e, string userID = null, string message = null, string module = null, string function = null)
        {
            Time = DateTime.Now;
            Module = $"WA-{module ?? e.TargetSite.DeclaringType.Name}";
            Function = function ?? e.TargetSite.Name;
            ExceptionMessage = e.Message;
            InnerExceptionMessage = e.InnerException?.Message ?? string.Empty;
            Message = message ?? string.Empty;
            User = userID ?? "WA";
            ExceptionType = e.GetType().Name;
        }
    }
}

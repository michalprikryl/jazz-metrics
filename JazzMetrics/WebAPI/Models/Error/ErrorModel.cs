using System;

namespace WebAPI.Models.Error
{
    /// <summary>
    /// trida, reprezentujici chybu urcenou k dalsimu zpracovani (ulozeni do DB, pripadne poslani emailem aj.)
    /// </summary>
    public class ErrorModel
    {
        /// <summary>
        /// cas vzniku chyby /// Time
        /// </summary>
        public DateTime? Time { get; set; }
        /// <summary>
        /// modul, kde vznikla chyba (nejcasteji trida) /// Module
        /// </summary>
        public string Module { get; set; }
        /// <summary>
        /// metoda/funkce, kde vznikla chyba /// Function
        /// </summary>
        public string Function { get; set; }
        /// <summary>
        /// chybova zprava s popisem /// Exception
        /// </summary>
        public string ExceptionMessage { get; set; }
        /// <summary>
        /// chybova zprava s popisem od inner exception naseho exception /// InnerException
        /// </summary>
        public string InnerExceptionMessage { get; set; }
        /// <summary>
        /// zprava s dalsim popisem/zpravou (casto custom popis programora) /// Message
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// uzivatel, pod kterym chyba vznikla /// User
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// typ exception (ApplicationException, Exception, IndexOutOfRangeException etc.)
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// kontruktor pro deserializaci objektu - NUTNY - jinak nelze objekt deserializovat
        /// </summary>
        public ErrorModel() { }

        /// <summary>
        /// konstruktor pro rucni zadani - bez exception (parametry odpovidaji properties tridy)
        /// </summary>
        /// <param name="module">modul, kde vznikla chyba (nejcasteji trida)</param>
        /// <param name="function">metoda/funkce, kde vznikla chyba</param>
        /// <param name="exceptionMessage">chybova zprava s popisem</param>
        /// <param name="innerExceptionMessage">chybova zprava s popisem od inner exception naseho exception</param>
        /// <param name="message">zprava s dalsim popisem/zpravou (casto custom popis programora)</param>
        /// <param name="user">uzivatel, pod kterym chyba vznikla</param>
        /// <param name="exceptionType">typ exception (ApplicationException, Exception, IndexOutOfRangeException etc.)</param>
        /// <param name="time">cas vzniku chyby</param>
        public ErrorModel(string module, string function, string exceptionMessage, string innerExceptionMessage, string message, string user, string exceptionType, DateTime? time = null)
        {
            Module = module;
            Function = function;
            ExceptionMessage = exceptionMessage;
            InnerExceptionMessage = innerExceptionMessage;
            Message = message;
            User = user;
            ExceptionType = exceptionType;
            Time = time ?? DateTime.Now;
        }

        /// <summary>
        /// konstruktor pro naplneni objektu pomoci vyjimky
        /// </summary>
        /// <param name="e">vyjimka, ze ktere chci naplnit objekt</param>
        /// <param name="userID">uzivatel, pod kterym chyba vznikla</param>
        /// <param name="message">zprava s dalsim popisem/zpravou (casto custom popis programora)</param>
        /// <param name="module">modul, kde vznikla chyba (nejcasteji trida)</param>
        /// <param name="function">metoda/funkce, kde vznikla chyba</param>
        public ErrorModel(Exception e, string userID = "API", string message = null, string module = null, string function = null)
        {
            Time = DateTime.Now;
            Module = module ?? e.TargetSite.DeclaringType.FullName.Split('+')[0];
            Function = function ?? $"{e.TargetSite.DeclaringType.Name} // {e.TargetSite.Name}";
            InnerExceptionMessage = e.InnerException?.Message ?? string.Empty;
            Message = message ?? string.Empty;
            ExceptionType = e.GetType().Name;
            ExceptionMessage = e.Message;
            User = userID;
        }
    }
}

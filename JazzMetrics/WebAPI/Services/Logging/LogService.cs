using Library;
using System;
using System.IO;
using System.Linq;

namespace WebAPI.Services.Log
{
    /// <summary>
    /// servis pro logovani
    /// </summary>
    public class LogService : ILogService
    {
        /// <summary>
        /// nazev souboru s error logem - slouzi pro zapis chyby v pripade, kdy se nepodarilo odeslat email s chybou (HelperService.cs)
        /// </summary>
        public static string ERROR_LOG = "error_log.txt";

        /// <summary>
        /// zapis libovolneho poctu radku do souboru
        /// </summary>
        /// <param name="file">nazev souboru</param>
        /// <param name="line">dany radek/radky k zapisu</param>
        /// <returns></returns>
        public bool WriteToFile(string file, params string[] lines)
        {
            try
            {
                File.AppendAllLines($"{Extensions.PATH}{file}", lines.Select(l => $"{DateTime.Now.GetDateTimeString()} → {l}"));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

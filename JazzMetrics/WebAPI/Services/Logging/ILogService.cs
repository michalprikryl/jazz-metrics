namespace WebAPI.Services.Log
{
    /// <summary>
    /// interface pro servis pro logovani
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// zapis libovolneho poctu radku do souboru
        /// </summary>
        /// <param name="file">nazev souboru</param>
        /// <param name="line">dany radek/radky k zapisu</param>
        /// <returns></returns>
        bool WriteToFile(string file, params string[] lines);
    }
}

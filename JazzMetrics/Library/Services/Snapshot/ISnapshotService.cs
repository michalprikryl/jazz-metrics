using System.Threading.Tasks;

namespace Library.Services.Snapshot
{
    /// <summary>
    /// interface pro servis na stahovani dat z Jazz
    /// </summary>
    public interface ISnapshotService
    {
        /// <summary>
        /// stahne a zpracuje data z Jazz
        /// </summary>
        /// <returns></returns>
        Task CreateSnapshots();
        /// <summary>
        /// zkontroluje, za jak dlouho se ma sluzba znovu spustit
        /// </summary>
        /// <returns></returns>
        Task<double> CheckPeriodSetting();
    }
}

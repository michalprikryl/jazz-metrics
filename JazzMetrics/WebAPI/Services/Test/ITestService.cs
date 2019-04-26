using Library.Models.Test;

namespace WebAPI.Services.Test
{
    /// <summary>
    /// interface pro servis pro testovani pripojeni k DB
    /// </summary>
    public interface ITestService
    {
        /// <summary>
        /// provede test pripojeni k DB
        /// </summary>
        /// <returns></returns>
        TestModel RunTest();
    }
}

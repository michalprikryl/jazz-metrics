namespace WebAPI.Services.Log
{
    public interface ILogService
    {
        bool WriteToFile(string file, params string[] lines);
    }
}

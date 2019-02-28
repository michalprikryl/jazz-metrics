namespace WebApp.Models
{
    public class ErrorViewModelOld
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
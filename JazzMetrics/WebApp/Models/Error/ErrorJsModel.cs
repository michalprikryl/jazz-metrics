namespace WebApp.Models.Error
{
    public class ErrorJsModel
    {
        public string Module { get; set; }
        public string Function { get; set; }
        public string Message { get; set; }
        public string InnerMessage { get; set; }
        public string ExceptionType { get; set; }
    }
}

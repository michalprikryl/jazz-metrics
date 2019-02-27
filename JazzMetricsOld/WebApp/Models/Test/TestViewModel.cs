namespace WebApp.Models.Test
{
    public class TestViewModel : ViewModel
    {
        public bool ConnectionDB { get; set; }
        public string MessageDB { get; set; }
        public bool ConnectionAPI { get; set; }
        public string MessageAPI { get; set; }
        public int HTTPResposeAPI { get; set; }
    }
}
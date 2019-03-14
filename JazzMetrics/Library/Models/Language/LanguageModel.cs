namespace Library.Models.Language
{
    public class LanguageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iso6391code { get; set; }
        public string Iso6393code { get; set; }

        public override string ToString() => $"{Name} ({Iso6391code})";
    }
}

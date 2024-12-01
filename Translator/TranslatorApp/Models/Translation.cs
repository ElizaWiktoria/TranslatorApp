namespace TranslatorApp.Models
{
    public class Translation
    {
        public int Id { get; set; }
        public string UserInput { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string TranslatedText { get; set; } = string.Empty;
    }
}

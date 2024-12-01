namespace TranslatorApp.Clients.Translate
{
    public class TranslateResponse
    {
        public required Success Success { get; set; }
        public required Contents Contents { get; set; }
    }
    
    public class Success
    {
        public int Total { get; set; }
    }

    public class Contents
    {
        public string Translated { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Translation { get; set; } = string.Empty;
    }
}

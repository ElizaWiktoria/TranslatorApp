namespace TranslatorApp.Clients
{
    public interface IFunTranslationsClient
    {
        Task<string?> Translate(string userInput, string language);
    }
}

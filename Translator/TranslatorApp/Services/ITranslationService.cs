using TranslatorApp.Models;

namespace TranslatorApp.Services
{
    public interface ITranslationService
    {
        Task<string> Translate(string userInput, string language);
        IEnumerable<Translation> GetTranslationHistory(SearchParameters searchParameters);
    }
}

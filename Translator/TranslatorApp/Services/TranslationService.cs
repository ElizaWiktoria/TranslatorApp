using Microsoft.IdentityModel.Tokens;
using TranslatorApp.Clients;
using TranslatorApp.DataContext;
using TranslatorApp.Models;

namespace TranslatorApp.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly IFunTranslationsClient _funTranslationsClient;
        private readonly IDataContextDapper _dataContext;
        private readonly ILogger _logger;

        public TranslationService(ILogger logger, IFunTranslationsClient funTranslationsClient, IDataContextDapper dataContext)
        {
            _dataContext = dataContext;
            _funTranslationsClient = funTranslationsClient;
            _logger = logger;
        }

        public async Task<string> Translate(string userInput, string language)
        {
            if (userInput.IsNullOrEmpty())
            {
                return "You need to specify text for translation.";
            }

            var translatedText = await _funTranslationsClient.Translate(userInput, language);
            if (translatedText == null)
            {
                return "Translation was unsuccessful due to translation service problems. Please try again later.";
            }

            try
            {
                const string sql =
                    @"INSERT INTO [dbo].[Translations]
                       ([UserInput]
                       ,[Language]
                       ,[TranslatedText])
                 VALUES
                       (@UserInput
                       ,@Language
                       ,@TranslatedText)";

                var parameters = new
                {
                    UserInput = userInput,
                    Language = language,
                    TranslatedText = translatedText
                };

                _dataContext.ExecuteSql(sql, parameters);

                return $"Translation: {translatedText}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving translation to database.");
                return $"Translation: {translatedText}";
            }
        }

        public IEnumerable<Translation> GetTranslationHistory(SearchParameters searchParameters)
        {
            try
            {
                var translations = _dataContext.LoadData<Translation>(searchParameters);

                return translations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving translation history from database.");
                return [];
            }
        }
    }
}

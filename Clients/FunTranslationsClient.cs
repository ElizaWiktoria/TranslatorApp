using Azure.Core;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using TranslatorApp.Clients.Translate;

namespace TranslatorApp.Clients
{
    public class FunTranslationsClient : IFunTranslationsClient
    {
        private readonly ILogger _logger;

        public FunTranslationsClient(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string?> Translate(string userInput, string language)
        {
            try
            {
                string url = $"https://api.funtranslations.com/translate/{language}.json";

                var jsonContent = JsonConvert.SerializeObject(new { text = userInput });
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Request URL: {url}\nMethod: POST\nResponse: {jsonResponse}");

                var apiResponse = JsonConvert.DeserializeObject<TranslateResponse>(jsonResponse);
                if (apiResponse == null)
                {
                    throw new Exception();
                }

                return apiResponse.Contents.Translated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting response from api.");
                return null;
            }
        }
    }
}

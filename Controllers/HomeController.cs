using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TranslatorApp.Models;
using TranslatorApp.Services;

namespace TranslatorApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITranslationService _translatationService;

        public HomeController(ILogger<HomeController> logger, ITranslationService translatationService)
        {
            _logger = logger;
            _translatationService = translatationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult History(string? userInputSearch = null, string? languageSearch = null, string? translatedTextSearch = null)
        {
            try
            {
                var searchParameters = new SearchParameters()
                {
                    UserInput = userInputSearch,
                    Language = languageSearch,
                    TranslatedText = translatedTextSearch
                };

                var translations = _translatationService.GetTranslationHistory(searchParameters);
                return View(new { translations });
            }   
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving translation history.");
                return Error();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Submit(string userInput, string language)
        {
            try
            {
                var message = await _translatationService.Translate(userInput, language);

                ViewData["Message"] = message;
                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while translating text.");
                return Error();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

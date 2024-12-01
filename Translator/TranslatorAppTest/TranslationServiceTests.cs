using Microsoft.Extensions.Logging;
using Moq;
using TranslatorApp.Clients;
using TranslatorApp.DataContext;
using TranslatorApp.Models;
using TranslatorApp.Services;

namespace TranslatorAppTest
{
    [TestClass]
    public class TranslationServiceTests
    {
        private ITranslationService _translationService;
        private readonly Mock<IFunTranslationsClient> _funTranslationsClientMock = new();
        private readonly Mock<IDataContextDapper> _dataContextDapperMock = new();

        [TestInitialize]
        public void Initialize()
        {
            _translationService = new TranslationService(Mock.Of<ILogger>(), _funTranslationsClientMock.Object, _dataContextDapperMock.Object);
        }

        [TestMethod]
        public async Task Translate_EmptyUserInput_ReturnsEmptyUserMessage()
        {
            //Arrange
            var userInput = "";
            var language = "LeetSpeak";

            //Act
            var message = await _translationService.Translate(userInput, language);

            //Assert
            Assert.AreEqual("You need to specify text for translation.", message);
        }

        [TestMethod]
        public async Task Translate_TranlsationServiceProblem_ReturnsServiceProblemsMessage()
        {
            //Arrange
            var userInput = "Test";
            var language = "LeetSpeak";

            var mockFunTranslationsClient = new Mock<IFunTranslationsClient>();
            mockFunTranslationsClient
                .Setup(client => client.Translate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string userInput, string language) => null);

            //Act
            var message = await _translationService.Translate(userInput, language);

            //Assert
            Assert.AreEqual("Translation was unsuccessful due to translation service problems. Please try again later.", message);
        }

        [TestMethod]
        public async Task Translate_CorrectUserInput_ReturnsTranslatedText()
        {
            //Arrange
            var userInput = "Text to translate";
            var language = "LeetSpeak";

            _funTranslationsClientMock
                .Setup(client => client.Translate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync("Text translated");

            //Act
            var message = await _translationService.Translate(userInput, language);

            //Assert
            Assert.AreEqual("Translation: Text translated", message);
        }

        [TestMethod]
        public async Task Translate_UnableToSaveToDb_ReturnsTranslatedText()
        {
            //Arrange
            var userInput = "Text to translate";
            var language = "LeetSpeak";

            _funTranslationsClientMock
                .Setup(client => client.Translate(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((string userInput, string language) => "Text translated");

            _dataContextDapperMock
                .Setup(db => db.ExecuteSql(It.IsAny<string>(), It.IsAny<object>()))
                .Throws(new Exception());

            //Act
            var message = await _translationService.Translate(userInput, language);

            //Assert
            Assert.AreEqual("Translation: Text translated", message);
        }

        [TestMethod]
        public void GetTranslationHistory_UnableRetrieveFromDb_ReturnsEmptyList()
        {
            //Arrange
            var searchParameters = new SearchParameters
            {
                Language = null,
                TranslatedText = null,
                UserInput = null
            };

            _dataContextDapperMock
                .Setup(db => db.LoadData<Translation>(It.IsAny<SearchParameters>()))
                .Throws(new Exception());

            //Act
            var translations = _translationService.GetTranslationHistory(searchParameters);

            //Assert
            Assert.AreEqual(false, translations.Any()); ;
        }

        [TestMethod]
        public void GetTranslationHistory_CorrectInput_ReturnsEmptyList()
        {
            //Arrange
            var searchParameters = new SearchParameters
            {
                Language = null,
                TranslatedText = null,
                UserInput = null
            };
            var translations = new List<Translation>
            {
                new() {
                    Id = 1,
                    Language = "LeetSpeak",
                    TranslatedText = "Text translated",
                    UserInput = "Text to translate"
                }
            };

            _dataContextDapperMock
                .Setup(db => db.LoadData<Translation>(It.IsAny<SearchParameters>()))
                .Returns(translations);

            //Act
            var translationsReturned = _translationService.GetTranslationHistory(searchParameters);

            //Assert
            Assert.AreEqual(true, translationsReturned.Any()); ;
        }
    }
}
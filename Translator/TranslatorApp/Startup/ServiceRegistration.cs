using TranslatorApp.Clients;
using TranslatorApp.Controllers;
using TranslatorApp.DataContext;
using TranslatorApp.Services;

namespace TranslatorApp.Startup
{
    public class ServiceRegistration
    {
        public static void Register(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var serviceProvider = services.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILogger<HomeController>>();
            services.AddSingleton(typeof(ILogger), logger);

            services.AddScoped<ITranslationService, TranslationService>();
            services.AddScoped<IFunTranslationsClient, FunTranslationsClient>();
            services.AddScoped<IDataContextDapper, DataContextDapper>();
        }
    }
}

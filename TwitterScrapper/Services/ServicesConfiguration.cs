using TwitterScrapper.Services.Scrapper;

namespace TwitterScrapper.Services
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IScrapper, Scrapper.Scrapper>();
            return services;
        }
    }
}

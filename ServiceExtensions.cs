using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnet_client
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDistenkaApi(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            var distenkaSettings = new ClientSettings();
            configuration.GetSection(Constants.DistenkaConfigSection).Bind(distenkaSettings);

            services.AddSingleton(distenkaSettings);

            services.AddSingleton<IAuthenticationProvider>(s => new ApiKeyProvider(distenkaSettings.APIKey));

            services.AddHttpClient(Constants.HttpClientName, (s, client) =>
            {
                var settings = s.GetRequiredService<ClientSettings>();
                client.BaseAddress = new Uri(string.Format(Constants.ApiEndpointTemplate, settings.OrgName, settings.Env));
            });

            services.AddTransient<IDistenkaClient>(s =>
            {
                var factory = s.GetRequiredService<IHttpClientFactory>();
                var settings = s.GetRequiredService<ClientSettings>();
                return new HttpRunClient(factory.CreateClient(Constants.HttpClientName), s.GetRequiredService<IAuthenticationProvider>(), settings);
            });

            return services;
        }
    }
}

using System.Threading.Tasks;
using Direct.Client.Interfaces;
using Direct.Client.Providers;
using Direct.Client.Services;
using Direct.Client;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Direct.Client.Extensions;
using Direct.Client.Helpers;

namespace Direct.Parser
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var directClient = serviceProvider.GetService<DirectClient>();
            //Получить все рекламные компании
            var allAampaigns = await directClient.GetAllCampaigns();
            //Получить все группы объявлений во всех компаниях
            var allAdGroups = await directClient.GetAllAdGroups();
            //Получить все объявления в аккаунте
            var allAds = await directClient.GetAllAds();
        }

        private static void RegisterDependencies(IServiceCollection serviceCollection) {
            serviceCollection
                .AddSingleton(_ => DirectParserLogger.Create())
                .AddSingleton<IAuthTokenProvider, AuthTokenProvider>()
                .AddSingleton<IUriProvider, DirectApiSandboxUrlProvider>()
                .AddSingleton<HttpClient>()
                .AddSingleton<SafeJsonSerializer>()
                .AddSingleton<DirectHttpRequestBuilder>()
                .AddSingleton<DirectRequestSender>()
                .AddSingleton<CampaignsService>()
                .AddSingleton<AdGroupsService>()
                .AddSingleton<AdsService>()
                .AddSingleton<DirectClient>();
        }
    }
}

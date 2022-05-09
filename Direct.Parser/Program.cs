using System;
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

            //while (true) {
                var directClient = serviceProvider.GetService<DirectClient>();
                var list = await directClient.GetCampaignsList();
                //Console.WriteLine(list);
                //await Task.Delay(5000);
            //}
        }

        private static void RegisterDependencies(IServiceCollection serviceCollection) {
            serviceCollection
                .AddSingleton(_ => DirectParserLogger.Create())
                .AddSingleton<IAuthTokenProvider, AuthTokenProvider>()
                .AddSingleton<IUriProvider, DirectApiSandboxUrlProvider>()
                .AddSingleton<HttpClient>()
                .AddSingleton<SafeJsonSerializer>()
                .AddSingleton<DirectHttpRequestBuilder>()
                .AddSingleton<CampaignsService>()
                .AddSingleton<DirectClient>();
        }
    }
}

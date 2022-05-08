using System;
using Direct.Client.Interfaces;
using Direct.Client.Providers;
using Direct.Client.Services;
using Direct.Client;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Direct.Parser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddSingleton(_ => DirectParserLogger.Create())
                .AddSingleton<IAuthTokenProvider, AuthTokenProvider>()
                .AddSingleton<IUriProvider, DirectApiSandboxUrlProvider>()
                .AddSingleton<HttpClient>()
                .AddSingleton<CampaignsService>()
                .AddSingleton<DirectClient>()
                .BuildServiceProvider();

            var directClient = serviceProvider.GetService<DirectClient>();
            var list = directClient.GetCampaignsList().GetAwaiter().GetResult();
            Console.WriteLine(list);
        }
    }
}

using System.Threading.Tasks;
using Direct.Client.Interfaces;
using Direct.Client.Providers;
using Direct.Client.Services;
using Direct.Client;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Direct.Client.Extensions;
using Direct.Client.Helpers;
using Vostok.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System;

namespace Direct.Parser
{
    public class DirectParserService : BackgroundService, IDisposable
    {
        private readonly CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();

        private readonly ServiceProvider serviceProvider;
        private readonly DirectClient directClient;
        private readonly ILog log;
        public DirectParserService() {
            var serviceCollection = new ServiceCollection();
            RegisterDependencies(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
            log = serviceProvider.GetService<ILog>();
            directClient = serviceProvider.GetService<DirectClient>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);
            log.Info($"DirectParserService is starting.");

            stoppingToken.Register(() =>
                log.Info($" DirectParser background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                log.Info($"DirectParser task doing background work.");

                // This eShopOnContainers method is querying a database table
                // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                await ParseAds();

                await Task.Delay(10000, stoppingToken);
            }

            log.Info($"DirectParser background task is stopping.");
        }
    
        public async Task ParseAds()
        {
            //Получить все рекламные компании
            var allCampaigns = await directClient.GetAllCampaigns();
            log.Info("Ads parsed!!!");
            //Получить все группы объявлений во всех компаниях
            //var allAdGroups = await directClient.GetAllAdGroups();
            //Получить все объявления в аккаунте
            //var allAds = await directClient.GetAllAds();
        }

        private static void RegisterDependencies(IServiceCollection serviceCollection) {
            serviceCollection
                .AddSingleton<ILog, DirectParserLogger>()
                .AddSingleton<IAuthTokenProvider>(_ => new AuthTokenProvider(Constants.AUTH_TOKEN))
                .AddSingleton<IUriProvider, DirectApiSandboxUrlProvider>()
                .AddSingleton<HttpClient>()
                .AddSingleton<SafeJsonResponseDeserializer>()
                .AddSingleton<DirectHttpRequestBuilder>()
                .AddSingleton<DirectRequestSender>()
                .AddSingleton<CampaignsService>()
                .AddSingleton<AdGroupsService>()
                .AddSingleton<AdsService>()
                .AddSingleton<DirectClient>();
        }

        public override void Dispose() {
            _stoppingCts.Cancel();
            this.serviceProvider.Dispose();
        }
    }
}

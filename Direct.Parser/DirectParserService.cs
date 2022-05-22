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
        private readonly DirectParser directParser;
        private readonly ILog log;
        public DirectParserService() {
            var serviceCollection = new ServiceCollection();
            RegisterDirectParser(serviceCollection);
            serviceProvider = serviceCollection.BuildServiceProvider();
            log = serviceProvider.GetService<ILog>();
            directParser = serviceProvider.GetService<DirectParser>();
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000);
            log.Info($"DirectParserService is starting.");

            stoppingToken.Register(() =>
                log.Info($" DirectParser background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                log.Info($"DirectParserService task doing background work.");
                await directParser.ParseAds();
                await Task.Delay(10000, stoppingToken);
            }

            log.Info($"DirectParserService background task is stopping.");
        }

        public static void RegisterDirectParser(IServiceCollection serviceCollection)
        {
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
                .AddSingleton<DirectClient>()
                .AddSingleton<DirectParser>();
        }

        public override void Dispose() {
            _stoppingCts.Cancel();
            this.serviceProvider.Dispose();
        }
    }
}

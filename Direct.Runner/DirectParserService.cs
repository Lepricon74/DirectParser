using System.Threading.Tasks;
using Vostok.Logging.Abstractions;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Direct.Parser.Database.Interfaces;
using System;
using Direct.Parser;

namespace Direct.Runner
{
    public class DirectParserService : BackgroundService, IDisposable
    {
        private readonly CancellationTokenSource _stoppingCts =
                                                   new CancellationTokenSource();
        private readonly DirectParser directParser;
        private readonly ILog log;
        private readonly Func<IAdsRepository> getAdsRepository;
        public DirectParserService(DirectParser directParser, ILog log, Func<IAdsRepository> getAdsRepository) {
            this.log = log;
            this.directParser = directParser;
            this.getAdsRepository = getAdsRepository;
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
                log.Info($"Task was started at " + DateTime.Now);
                await directParser.ParseAds(getAdsRepository());
                log.Info($"Task finished work at " + DateTime.Now);
                log.Info($"DirectParserService task waiting for start time...");
                await Task.Delay(10000, stoppingToken);
            }

            log.Info($"DirectParserService background task is stopping.");
        }
    }
}

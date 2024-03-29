﻿using System.Threading.Tasks;
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
        private readonly Func<IAdImagesRepository> getAdImagesRepository;
        private readonly TimeSpan cyclePeriod;
        public DirectParserService(DirectParser directParser, ILog log, Func<IAdsRepository> getAdsRepository, 
            Func<IAdImagesRepository> getAdImagesRepository, TimeSpan cyclePeriod) {
            this.log = log;
            this.directParser = directParser;
            this.getAdsRepository = getAdsRepository;
            this.cyclePeriod = cyclePeriod;
            this.getAdImagesRepository = getAdImagesRepository;
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
                await directParser.ParseAdImages(getAdImagesRepository());
                log.Info($"Task finished work at " + DateTime.Now);
                log.Info($"DirectParserService task waiting for start time...");
                await Task.Delay(cyclePeriod, stoppingToken);
            }

            log.Info($"DirectParserService background task is stopping.");
        }
    }
}

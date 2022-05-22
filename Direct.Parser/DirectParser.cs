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
    public class DirectParser
    {
        private readonly DirectClient directClient;
        private readonly ILog log;
        public DirectParser(DirectClient directClient, ILog log)
        {
            this.directClient = directClient;
            this.log = log;
        }
        
        public async Task ParseAds()
        {
            var allCampaigns = await directClient.GetAllCampaigns();
            foreach (var campaign in allCampaigns.Campaigns) {
                var campaingAds = await directClient.GetAllAds();
            }
            log.Info("Ads parsed!!!");
            //Получить все группы объявлений во всех компаниях
            //var allAdGroups = await directClient.GetAllAdGroups();
            //Получить все объявления в аккаунте
            //var allAds = await directClient.GetAllAds();
        }
    }
}

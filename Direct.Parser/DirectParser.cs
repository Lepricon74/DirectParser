﻿using System.Threading.Tasks;
using Direct.Client;
using Vostok.Logging.Abstractions;
using System;
using Direct.Parser.Database.Models;
using Direct.Parser.Database.Interfaces;

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
        
        public async Task ParseAds(IAdsRepository adsRepository)
        {
            log.Info("START-PARSE-ADS-LIST");
            var ads = await directClient.GetAllAds();
            foreach (var ad in ads.Ads) {
                var adPromotionEndDate = await TryGetAdPromotionEnd(ad.TextAd.Text);
                var adForUpdate = new Ad(ad.Id, ad.AdGroupId, ad.CampaignId, ad.Type, ad.Status, ad.TextAd.Text, ad.TextAd.Title, adPromotionEndDate); 
                await adsRepository.AddOrUpdateAd(adForUpdate);
            }
            log.Info($"FINISH-PARSE-ADS-LIST");
        }

        private async Task<DateTime?> TryGetAdPromotionEnd(string adText) 
        {
            //var gen = new Random();
            //var result = DateTime.Today.AddDays(gen.Next(0, 60));
            //return result;
            return await DateParser.GetDateTimeFromText(adText);
        }
    }
}

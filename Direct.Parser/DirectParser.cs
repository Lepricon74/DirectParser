using System.Threading.Tasks;
using Direct.Client;
using Vostok.Logging.Abstractions;
using System;
using Direct.Parser.Database.Models;
using Direct.Parser.Database.Interfaces;
using Direct.Client.Models.Ads;

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
            AdsResponseResult ads = default;
            try
            {
                ads = await directClient.GetAllAds();
            }
            catch (Exception ex) {
                log.Error($"GetAllAds Fail: " + ex.Message);
            }
            if (ads != default)
            {
                foreach (var ad in ads.Ads)
                {
                    if (ad.Type!="TEXT_AD") continue;
                    var adTextPromotionEndDate = await TryGetAdPromotionEnd(ad.TextAd.Text);
                    var adTitlePromotionEndDate = await TryGetAdPromotionEnd(ad.TextAd.Title);
                    DateTime?[] resusltDates = new DateTime?[2] { adTextPromotionEndDate, adTitlePromotionEndDate };       
                    var adForUpdate = new Ad(ad.Id, ad.AdGroupId, ad.CampaignId, ad.Type, ad.Status, ad.TextAd.Text, ad.TextAd.Title, resusltDates);
                    await adsRepository.AddOrUpdateAd(adForUpdate);
                }
            }
            else {
                log.Warn($"Ads List was empty.");
            }
            log.Info($"FINISH-PARSE-ADS-LIST");
        }

        private async Task<DateTime?> TryGetAdPromotionEnd(string adText) 
        {
            return await DateParser.GetDateTimeFromText(adText);
        }
    }
}

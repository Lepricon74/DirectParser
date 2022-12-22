using System.Threading.Tasks;
using Direct.Client;
using Vostok.Logging.Abstractions;
using System;
using System.Linq;
using Direct.Client.Models.AdImages;
using Direct.Parser.Database.Models;
using Direct.Parser.Database.Interfaces;
using Direct.Client.Models.Ads;
using Direct.ImageRecognitionClient;

namespace Direct.Parser
{
    public class DirectParser
    {
        private readonly DirectClient directClient;
        private readonly ILog log;
        private readonly ImageToTextRecognitionClient imageToTextRecognitionClient;

        public DirectParser(DirectClient directClient, ILog log, ImageToTextRecognitionClient imageToTextRecognitionClient)
        {
            this.directClient = directClient;
            this.log = log;
            this.imageToTextRecognitionClient = imageToTextRecognitionClient;
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
                    DateTime?[] resultDates = new DateTime?[2] { adTextPromotionEndDate, adTitlePromotionEndDate };       
                    var adForUpdate = new Ad(ad.Id, ad.AdGroupId, ad.CampaignId, ad.Type, ad.Status, ad.TextAd.Text, ad.TextAd.Title, resultDates);
                    await adsRepository.AddOrUpdateAd(adForUpdate);
                }
            }
            else {
                log.Warn($"Ads List was empty.");
            }
            log.Info($"FINISH-PARSE-ADS-LIST");
        }

        public async Task ParseAdImages(IAdImagesRepository adImagesRepository)
        {
            log.Info("PARSE-IMAGES-LIST");
            AdImagesResponseResult images = default;
            try
            {
                images = await directClient.GetAllImages(Array.Empty<string>(), "YES");
            }
            catch (Exception ex)
            {
                log.Error($"GetAllImages Fail: " + ex.Message);
            }

            if (images != default)
            {
                var existingImages = await adImagesRepository.GetAdImagesList();
                var newImages = images.AdImages.Where(im => existingImages.All(ei => ei.ImageHash != im.AdImageHash))
                    .ToList();
                var imageRecognizedText  = await imageToTextRecognitionClient.ImagesToText(newImages);
                foreach (var image in newImages)
                {
                    var imageText = imageRecognizedText.First(im => im.ImageHash == image.AdImageHash).ImageText;
                    var adImagePromotionEndDate = await TryGetAdPromotionEnd(imageText);
                    var adImage = new AdImage(image.AdImageHash, image.OriginalUrl, imageText, adImagePromotionEndDate);
                    await adImagesRepository.AddAdImage(adImage);
                }
            }
        }

        private async Task<DateTime?> TryGetAdPromotionEnd(string adText) 
        {
            return await DateParser.GetDateTimeFromText(adText);
        }
    }
}

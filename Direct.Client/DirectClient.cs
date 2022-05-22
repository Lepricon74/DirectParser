using System.Threading.Tasks;
using Direct.Client.Models.Campaings;
using Direct.Client.Models.AdGroups;
using Direct.Client.Models.Ads;
using Direct.Client.Services;
using Vostok.Logging.Abstractions;
using System.Linq;

namespace Direct.Client
{
    public class DirectClient
    {
        public CampaignsService campaingsService;
        public AdGroupsService adGroupsService;
        public AdsService adsService;
        public ILog log;

        public DirectClient(
            ILog log, 
            CampaignsService campaingsService, 
            AdGroupsService adGroupsService, 
            AdsService adsService) {
            this.log = log;
            this.campaingsService = campaingsService;
            this.adGroupsService = adGroupsService;
            this.adsService = adsService;
        }

        public async Task<CampaignsResponseResult> GetAllCampaigns() {
            return await campaingsService.GetAllCampaigns();
        }

        public async Task<AdGroupsResponseResult> GetAllAdGroups()
        {
            var campaigns = await campaingsService.GetAllCampaigns(
                new string[]{CampaignsService.AvailableRequestFieldNames.Id.ToString()});
            var campaignsIds = campaigns?.Campaigns.Select(campaign => campaign.Id).ToArray();
            return await adGroupsService.GetAdGroups(campaignsIds);
        }
        public async Task<AdsResponseResult> GetAllAds()
        {
            var campaigns = await campaingsService.GetAllCampaigns(
                new string[] { CampaignsService.AvailableRequestFieldNames.Id.ToString() });
            var campaignsIds = campaigns?.Campaigns.Select(campaign => campaign.Id).ToArray();
            return await adsService.GetAds(campaignsIds, new long[] { });
        }
    }
}

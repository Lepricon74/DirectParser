using System.Threading.Tasks;
using Direct.Client.Models.Campaings;
using Direct.Client.Services;
using Vostok.Logging.Abstractions;

namespace Direct.Client
{
    public class DirectClient
    {
        public CampaignsService campaingsService;
        public ILog log;

        public DirectClient(ILog log, CampaignsService campaingsService) { 
            this.log = log;
            this.campaingsService = campaingsService;
        }

        public async Task<CampaignsResponseResult> GetCampaignsList() {
            return await campaingsService.GetAllCampaigns();
        }
    }
}

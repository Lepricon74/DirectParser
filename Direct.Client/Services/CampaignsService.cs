using System;
using System.Threading.Tasks;
using Direct.Client.Models;
using Direct.Client.Models.Campaings;
using Direct.Client.Interfaces;
using Direct.Client.Helpers;
using Direct.Client.Models.Campaigns;

namespace Direct.Client.Services
{
    public class CampaignsService
    {
        private IUriProvider uriProvider;
        private DirectRequestSender directRequestSender;

        public CampaignsService(
            DirectRequestSender directRequestSender,  
            IUriProvider uriProvider)
        {
            this.uriProvider = uriProvider;
            this.directRequestSender = directRequestSender;
        }

        public enum AvailableRequestFieldNames
        {
            Id,
            Name,
            StartDate,
            EndDate,
            Type,
            Status
        }

        private Uri GetUriToCampaingsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/campaigns");
        }

        public async Task<CampaignsResponseResult> GetAllCampaigns(string[] selectFields = null)
        {
            var actionName = "GET-ALL-CAMPAINGS";
            CommonRequestParams<CampaignsRequestSelectionCriteria> GetRequestParams(){
                return new CommonRequestParams<CampaignsRequestSelectionCriteria>(
                    new CampaignsRequestSelectionCriteria(new int[] { }),
                    (selectFields == null) ? Enum.GetNames(typeof(AvailableRequestFieldNames)) : selectFields);
            }
            var campaignsResponseResult = await directRequestSender.SendDirectGetRequest<
                CommonRequestParams<CampaignsRequestSelectionCriteria>,
                CampaignsResponseResult>(
                    GetRequestParams,
                    GetUriToCampaingsService,
                    actionName);
            return campaignsResponseResult;
        }
    }
}

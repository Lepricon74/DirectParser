using System;
using System.Threading.Tasks;
using Direct.Client.Models;
using Direct.Client.Interfaces;
using Direct.Client.Helpers;
using Direct.Client.Models.AdGroups;

namespace Direct.Client.Services
{
    public class AdGroupsService
    {
        private IUriProvider uriProvider;
        private DirectRequestSender directRequestSender;

        public AdGroupsService(
            IUriProvider uriProvider, 
            DirectRequestSender directRequestSender)
        {
            this.uriProvider = uriProvider;
            this.directRequestSender = directRequestSender;
        }

        public enum AvailableRequestFieldNames
        {
            Id,
            Name,
            CampaignId,
            Type,
            Status
        }

        private Uri GetUriToAdGroupsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/adgroups");
        }

        public async Task<AdGroupsResponseResult> GetAdGroups(long[] CampaignIds)
        {
            var actionName = "GET-ALL-ADGROUPS";
            DirectRequest<CommonRequestParams<AdGroupsRequestSelectionCriteria>> GetRequestContent()
            {
                return new DirectRequest<CommonRequestParams<AdGroupsRequestSelectionCriteria>>(
                    "get",
                    new CommonRequestParams<AdGroupsRequestSelectionCriteria>(
                        new AdGroupsRequestSelectionCriteria(new long[] { }, CampaignIds),
                        Enum.GetNames(typeof(AvailableRequestFieldNames)))
                );
            }
            var campaignsResponseResult = await directRequestSender.SendDirectRequest<
                DirectRequest<CommonRequestParams<AdGroupsRequestSelectionCriteria>>,
                DirectResponse<AdGroupsResponseResult>>(
                    GetRequestContent,
                    GetUriToAdGroupsService,
                    actionName);
            return campaignsResponseResult.result;
        }
    }
}

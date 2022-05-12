using System;
using System.Threading.Tasks;
using Direct.Client.Models;
using Direct.Client.Interfaces;
using Direct.Client.Helpers;
using Direct.Client.Models.Ads;

namespace Direct.Client.Services
{
    public class AdsService
    {
        private IUriProvider uriProvider;
        private DirectRequestSender directRequestSender;

        public AdsService(
            DirectRequestSender directRequestSender,
            IUriProvider uriProvider)
        {
            this.uriProvider = uriProvider;
            this.directRequestSender = directRequestSender;
        }

        public enum AvailableRequestFieldNames
        {
            Id,
            AdGroupId,
            CampaignId,
            Type,
            Status
        }
        public enum AvailableRequestTextAdFieldNames
        {
            Title,
            Title2,
            Text
        }

        private Uri GetUriToAdGroupsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/ads");
        }

        public async Task<AdsResponseResult> GetAds(
            long[] CampaignIds, 
            long[] AdGroupsIds, 
            string[] selectFields = null, 
            string[] selectTextAdFields = null)
        {
            var actionName = "GET-ALL-ADS";
            DirectRequest<CommonRequestParams<AdsRequestSelectionCriteria>> GetRequestContent()
            {
                return new DirectRequest<CommonRequestParams<AdsRequestSelectionCriteria>>(
                    "get",
                    new AdsRequestParams<AdsRequestSelectionCriteria>(
                        new AdsRequestSelectionCriteria(new long[] { }, AdGroupsIds, CampaignIds),
                        selectFields == null ? Enum.GetNames(typeof(AvailableRequestFieldNames)) : selectFields,
                        selectTextAdFields == null ? Enum.GetNames(typeof(AvailableRequestTextAdFieldNames)) : selectTextAdFields)
                    );
            }
            var campaignsResponseResult = await directRequestSender.SendDirectRequest<
                DirectRequest<CommonRequestParams<AdsRequestSelectionCriteria>>,
                DirectResponse<AdsResponseResult>>(
                    GetRequestContent,
                    GetUriToAdGroupsService,
                    actionName);
            return campaignsResponseResult.result;
        }
    }
}

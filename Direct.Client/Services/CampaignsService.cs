using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Direct.Client.Models;
using Direct.Client.Models.Campaings;
using Direct.Client.Interfaces;
using Vostok.Logging.Abstractions;
using System.Text.Json;
using Direct.Client.Extensions;
using Direct.Client.Models.Errors;
using Direct.Client.Helpers;
using Direct.Client.Models.Campaigns;

namespace Direct.Client.Services
{
    public class CampaignsService
    {
        private ILog log;
        private HttpClient httpClient;
        private IUriProvider uriProvider;
        private SafeJsonSerializer serializer;
        private DirectHttpRequestBuilder requestBuilder;

        public CampaignsService(
            ILog log, 
            HttpClient httpClient, 
            IUriProvider uriProvider, 
            SafeJsonSerializer serializer,
            DirectHttpRequestBuilder requestBuilder)
        {
            this.log = log;
            this.httpClient = httpClient;
            this.uriProvider = uriProvider;
            this.serializer = serializer;
            this.requestBuilder = requestBuilder;
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

        public async Task<CampaignsResponseResult> GetAllCampaigns()
        {
            var actionName = "GET-ALL-CAMPAINGS";
            var request = requestBuilder.PrepareRequest(GetUriToCampaingsService);
            var requestContent = new DirectRequest<CampaignsRequestSelectionCriteria>(
                    "get",
                    new Params<CampaignsRequestSelectionCriteria>(
                        new CampaignsRequestSelectionCriteria(new int[] { }),
                        Enum.GetNames(typeof(AvailableRequestFieldNames))
                        )
                );
            request.Content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
            log.Info($"START-{actionName} Uri:{request.RequestUri}");
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializeResult = serializer.TryDirectResponseDeserialize<DirectResponse<CampaignsResponseResult>>(body, actionName);
            if (deserializeResult != null)
            {
                log.Info($"FINISH-{actionName} Uri:{request.RequestUri}");
                return deserializeResult?.result;
            }
            else return null;
        }
    }
}

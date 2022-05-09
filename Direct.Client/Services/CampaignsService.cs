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

        private Uri GetUriToCampaingsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/campaigns");
        }

        public async Task<CampaignsList> GetCampaignsList()
        {
            var request = requestBuilder.PrepareRequest(GetUriToCampaingsService);
            request.Content = new StringContent(@"{""method"": ""get"",""params"": {""SelectionCriteria"": { },""FieldNames"": [""Id"", ""Name""] }}", Encoding.UTF8, "application/json");
            log.Info($"START-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializeResult = serializer.TryDirectResponseDeserialize<DirectResponse<CampaignsList>>(body, "GET-CAMPAINGS-LIST");
            log.Info($"FINISH-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            return deserializeResult.result;
        }
    }
}

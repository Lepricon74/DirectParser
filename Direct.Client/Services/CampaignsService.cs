using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Direct.Client.Models;
using Direct.Client.Models.Campaings;
using Direct.Client.Interfaces;
using Vostok.Logging.Abstractions;

namespace Direct.Client.Services
{
    public class CampaignsService
    {
        private ILog log;
        private IAuthTokenProvider tokenProvider;
        private HttpClient httpClient;
        private IUriProvider uriProvider;

        public CampaignsService(ILog log, IAuthTokenProvider tokenProvider, HttpClient httpClient, IUriProvider uriProvider)
        {
            this.log = log;
            this.tokenProvider = tokenProvider;
            this.httpClient = httpClient;
            this.uriProvider = uriProvider;
        }

        public Uri GetUriToCampaingsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/campaigns");
        }

        public async Task<CampaignsList> GetCampaignsList()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = GetUriToCampaingsService(),
                Content = new StringContent(@"{""method"": ""get"",""params"": {""SelectionCriteria"": { },""FieldNames"": [""Id"", ""Name""] }}", Encoding.UTF8, "application/json"),
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.GetToken());
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            log.Info($"START-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializeResult = JsonConvert.DeserializeObject<DirectSuccessCommon<CampaignsList>>(body);
            log.Info($"FINISH-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            return deserializeResult.result;
        }
    }
}

using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
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
        private IAuthTokenProvider tokenProvider;
        private HttpClient httpClient;
        private IUriProvider uriProvider;
        private SafeJsonSerializer serializer;

        public CampaignsService(ILog log, IAuthTokenProvider tokenProvider, HttpClient httpClient, IUriProvider uriProvider, SafeJsonSerializer serializer)
        {
            this.log = log;
            this.tokenProvider = tokenProvider;
            this.httpClient = httpClient;
            this.uriProvider = uriProvider;
            this.serializer = serializer;
        }

        private Uri GetUriToCampaingsService() {
            return new Uri(uriProvider.GetUri().AbsoluteUri + "/campaigns");
        }

        public async Task<CampaignsList> GetCampaignsList()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = GetUriToCampaingsService(),
                //Content = new StringContent(@"{""method"": ""get"",""params"": {""SelectionCriteria"": { },""FieldNames"": [""Id"", ""Name""] }}", Encoding.UTF8, "application/json"),
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.GetToken());
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));

            log.Info($"START-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            var result = await httpClient.SendAsync(request).ConfigureAwait(false);
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            DirectResponse<CampaignsList> deserializeResult;
            try
            {
                deserializeResult = serializer.Deserialize<DirectResponse<CampaignsList>>(body);
            }
            catch (JsonException ex1)
            {
                try
                {
                    var directError = serializer.Deserialize<DirectError<RequestError>>(body);
                    log.Warn(ex1.Message);
                    log.Warn($"GET-CAMPAINGS-LIST-FAILED Reason:{directError.error.error_string} Request_ID:{directError.error.request_id} Error_Code:{directError.error.error_code}");
                }
                catch (JsonException ex2)
                {
                    log.Warn($"GET-CAMPAINGS-LIST-FAILED Reason:{ex2.Message}");
                }
                return null;
            }
            log.Info($"FINISH-GET-CAMPAINGS-LIST Uri:{request.RequestUri}");
            return deserializeResult.result;
        }
    }
}

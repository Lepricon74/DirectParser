using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Vostok.Logging.Abstractions;
using System.Text.Json;
using Direct.Client.Extensions;

namespace Direct.Client.Helpers
{
    public class DirectRequestSender
    {
        private ILog log;
        private HttpClient httpClient;
        private SafeJsonSerializer serializer;
        private DirectHttpRequestBuilder requestBuilder;

        public DirectRequestSender(
            ILog log,
            HttpClient httpClient,
            SafeJsonSerializer serializer,
            DirectHttpRequestBuilder requestBuilder)
        {
            this.log = log;
            this.httpClient = httpClient;
            this.serializer = serializer;
            this.requestBuilder = requestBuilder;
        }
        public async Task<ResponseType> SendDirectRequest<RequestType, ResponseType>(
            Func<RequestType> getRequestContent, 
            Func<Uri> getServiceUri, 
            string actionName) 
            where RequestType : class
            where ResponseType : class
        {
            var request = requestBuilder.PrepareRequest(getServiceUri);
            var requestContent = getRequestContent();
            request.Content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");
            log.Info($"START-{actionName} Uri:{request.RequestUri}");
            var result = await httpClient.SafeSendAsync(request, log);
            if (result == null)
            {
                log.Info($"{actionName}-FAILED Uri:{request.RequestUri}");
                return default;
            }
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializeResult = serializer.TryDirectResponseDeserialize<ResponseType>(body, actionName);
            if (deserializeResult != null)
            {
                log.Info($"FINISH-{actionName} Uri:{request.RequestUri}");
                return deserializeResult;
            }
            else return default;
        }
    }
}

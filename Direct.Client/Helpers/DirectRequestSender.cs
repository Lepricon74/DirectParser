using System;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Vostok.Logging.Abstractions;
using System.Text.Json;
using Direct.Client.Extensions;
using Direct.Client.Models;

namespace Direct.Client.Helpers
{
    public class DirectRequestSender
    {
        private const string LOG_PREFIX = "\t";

        private ILog log;
        private HttpClient httpClient;
        private SafeJsonResponseDeserializer serializer;
        private DirectHttpRequestBuilder requestBuilder;

        public DirectRequestSender(
            ILog log,
            HttpClient httpClient,
            SafeJsonResponseDeserializer serializer,
            DirectHttpRequestBuilder requestBuilder)
        {
            this.log = log;
            this.httpClient = httpClient;
            this.serializer = serializer;
            this.requestBuilder = requestBuilder;
        }
        public async Task<ResponseResultType> SendDirectGetRequest<RequestParams, ResponseResultType>(
            Func<RequestParams> getRequestParams, 
            Func<Uri> getServiceUri, 
            string actionName) 
            where RequestParams : class
            where ResponseResultType : class
        {
            var request = requestBuilder.PrepareRequest(getServiceUri);
            var requestContent = new DirectRequest<RequestParams>("get", getRequestParams());
            var jsonRequestContent = JsonSerializer.Serialize(requestContent);
            request.Content = new StringContent(jsonRequestContent, Encoding.UTF8, "application/json");

            log.Info($"START-REQUEST-{actionName}\n" +
                $"{LOG_PREFIX}Uri: {request.RequestUri}\n" +
                $"{LOG_PREFIX}RequestBody: {ConvertJsonToStringForPrint(jsonRequestContent, $"{LOG_PREFIX}")}");
            var result = await httpClient.SafeSendAsync(request, log);
            if (result == null)
            {
                log.Info($"{actionName}-FAILED Uri:{request.RequestUri}");
                return default;
            }
            var body = await result.Content.ReadAsStringAsync().ConfigureAwait(false);
            var deserializeResult = serializer.TryDirectResponseDeserialize<ResponseResultType>(body, actionName);
            if (deserializeResult != null)
            {
                log.Info($"FINISH-REQUEST-{actionName}");
                return deserializeResult.result;
            }
            else return default;
        }

        private string ConvertJsonToStringForPrint(string jsonString, string logPrefix) 
        {
            var sb = new StringBuilder();
            var isArray = false;
            string curPrefix = logPrefix+'\t';
            sb.Append('\n'+curPrefix);
            foreach (var symbol in jsonString)
            {
                if (symbol == '}')
                {
                    sb.Append("\n");
                    curPrefix = curPrefix.Remove(curPrefix.Length - 1);
                    sb.Append(curPrefix);
                }
                sb.Append(symbol);
                if (symbol == '{') {
                    sb.Append("\n");
                    curPrefix = curPrefix + "\t";
                    sb.Append(curPrefix);
                }  
                if (symbol == '[') isArray = true;
                if (symbol == ']') isArray = false;
                if (symbol == ',' && isArray == false)
                {
                    sb.Append("\n");
                    sb.Append(curPrefix);
                }
            }
            return sb.ToString();
        }
    }
}

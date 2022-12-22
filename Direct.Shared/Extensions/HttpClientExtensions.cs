using System.Net.Http;
using System.Threading.Tasks;
using Vostok.Logging.Abstractions;
namespace Direct.Shared.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SafeSendAsync(this HttpClient httpClient,HttpRequestMessage request, ILog log) {
            var result = new HttpResponseMessage();
            try
            {
                result = await httpClient.SendAsync(request).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                log.Warn($"HTTP-REQUEST-FAILED HttpRequestError:{ex.Message}");
                return null;
            }
            return result;
        }
    }
}

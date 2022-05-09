using Direct.Client.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System;

namespace Direct.Client.Helpers
{
    public class DirectHttpRequestBuilder
    {
        private IAuthTokenProvider tokenProvider;

        public DirectHttpRequestBuilder(IAuthTokenProvider tokenProvider) {
            this.tokenProvider = tokenProvider;
        }

        public HttpRequestMessage PrepareRequest( Func<Uri> getUriToService) {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = getUriToService(),
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.GetToken());
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            return request;
        }
    }
}

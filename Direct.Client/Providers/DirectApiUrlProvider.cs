using System;
using Direct.Client.Interfaces;
namespace Direct.Client.Providers
{
    public class DirectApiUrlProvider : IUriProvider
    {
        private readonly string uri;

        public DirectApiUrlProvider(string uri) { 
            this.uri = uri;
        }
        public Uri GetUri() 
        {
            return new Uri("https://api-sandbox.direct.yandex.com/json/v5");
        }
    }
}

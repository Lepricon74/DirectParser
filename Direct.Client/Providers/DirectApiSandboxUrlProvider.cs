using System;
using Direct.Client.Interfaces;
namespace Direct.Client.Providers
{
    public class DirectApiSandboxUrlProvider : IUriProvider
    {
        public Uri GetUri() 
        {
            return new Uri("https://api-sandbox.direct.yandex.com/json/v5");
        }
    }
}

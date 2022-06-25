using System;
using Direct.Client.Interfaces;
namespace Direct.Client.Providers
{
    public class DirectApiUrlProvider : IUriProvider
    {
        private readonly Uri uri;

        public DirectApiUrlProvider(Uri uri) { 
            this.uri = uri;
        }
        public Uri GetUri() 
        {
            return uri;
        }
    }
}

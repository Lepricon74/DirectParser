using Direct.Client.Interfaces;

namespace Direct.Client.Providers
{
    public class AuthTokenProvider : IAuthTokenProvider
    {
        private readonly string token;

        public AuthTokenProvider(string token) { 
            this.token = token;
        }
        public string GetToken() {
            return token;
        }
    }
}

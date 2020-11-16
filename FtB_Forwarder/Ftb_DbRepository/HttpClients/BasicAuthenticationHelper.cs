using System;
using System.Net.Http.Headers;
using System.Text;

namespace Ftb_Repositories.HttpClients
{
    public static class BasicAuthenticationHelper
    {
        public static AuthenticationHeaderValue GetAuthenticationHeader(FormProcessAPISettings settings)
        {
            var authToken = Encoding.ASCII.GetBytes($"{settings.BasicAuthUserName}:{settings.BasicAuthPassword}");
            return new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
        }
    }
}

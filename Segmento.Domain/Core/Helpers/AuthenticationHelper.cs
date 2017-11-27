using System.Web;
using Tweetinvi;
using Tweetinvi.Models;

namespace Segmento.Domain.Core.Helpers
{
    public static class AuthenticationHelper
    {
        public static IAuthenticatedUser ValidateUser(HttpRequestBase request)
        {
            Settings.CurrentUser = null;

            string verifierCode = request.Params.Get("oauth_verifier");
            string authorizationId = request.Params.Get("authorization_id");
            
            if (!string.IsNullOrEmpty(verifierCode))
            {
                var userCreds = AuthFlow.CreateCredentialsFromVerifierCode(verifierCode, authorizationId);
                var user = Tweetinvi.User.GetAuthenticatedUser(userCreds);

                Settings.CurrentUser = user;
                return user;
            }
            return null;
        }

        public static void SetCurrentUser(IAuthenticatedUser user)
        {
            Settings.CurrentUser = user;
        }

        public static ConsumerCredentials GetCreds()
        {
            return new ConsumerCredentials(Settings.ConsumerKey, Settings.ConsumerSecret);
        }
    }
}

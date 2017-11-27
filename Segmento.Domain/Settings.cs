using Tweetinvi;
using Tweetinvi.Models;

namespace Segmento.Domain
{
    public static class Settings
    {
        internal static string ConsumerKey { get; set; }
        internal static string ConsumerSecret { get; set; }
        internal static string UserAccessToken { get; set; }
        internal static string UserAccessSecret { get; set; }

        internal static IAuthenticatedUser CurrentUser { get; set; }

        public static void Initialize(string consumerKey, string consumerSecret, string userAccessToken, string userAccessSecret)
        {
            ConsumerKey = consumerKey;
            ConsumerSecret = consumerSecret;
            UserAccessToken = userAccessToken;
            UserAccessSecret = userAccessSecret;
            Auth.SetUserCredentials(consumerKey, consumerSecret, userAccessToken, userAccessSecret);            
        }
    }
}

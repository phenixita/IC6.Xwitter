using LinqToTwitter;

namespace IC6.Xwitter
{
    public class LinqToTwitterAuthorizer : ILinqToTwitterAuthorizer
    {
        public IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret)
        {
            return new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = consumerKey,

                    ConsumerSecret = consumerSecret,
                },
            };
        }

        public IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret, string oAuthToken, string oAuthTokenSecret)
        {
            var cred = new InMemoryCredentialStore
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                OAuthToken = oAuthToken,
                OAuthTokenSecret = oAuthTokenSecret
            };

            var auth0 = new PinAuthorizer()
            {
                CredentialStore = cred,
            };

            return auth0;
        }
    }
}
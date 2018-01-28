using LinqToTwitter;

namespace IC6.Xwitter
{
    public interface ILinqToTwitterAuthorizer
    {
        IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret);

        IAuthorizer GetAuthorizer(string consumerKey, string consumerSecret, string oAuthToken, string oAuthTokenSecret);

    }
}
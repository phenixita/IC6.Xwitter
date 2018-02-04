using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IC6.Xwitter
{
    class LoginStore : ILoginStore
    {
        private enum OAuth
        {
            OAuthToken, OAuthSecret
        }

        public UserSecrets GetSecrets()
        {
            string token = null;
            string secret = null;
            if (App.Current.Properties.ContainsKey(OAuth.OAuthToken.ToString()))
            {
                token = App.Current.Properties[OAuth.OAuthToken.ToString()].ToString();
            }

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            if (App.Current.Properties.ContainsKey(OAuth.OAuthSecret.ToString()))
            {
                secret = App.Current.Properties[OAuth.OAuthSecret.ToString()].ToString();
            }

            if (string.IsNullOrEmpty(secret))
            {
                return null;
            }

            return new UserSecrets() { OAuthSecret = secret, OAuthToken = token };
        }

        public async Task SetSecretsAsync(UserSecrets secrets)
        {
            await SetSecretsAsync(secrets.OAuthToken, secrets.OAuthSecret);
        }

        public async Task SetSecretsAsync(string oauthToken, string oauthSecret)
        {
            App.Current.Properties[OAuth.OAuthSecret.ToString()] = oauthSecret;
            App.Current.Properties[OAuth.OAuthToken.ToString()] = oauthToken;

            await App.Current.SavePropertiesAsync();
        }
    }
}

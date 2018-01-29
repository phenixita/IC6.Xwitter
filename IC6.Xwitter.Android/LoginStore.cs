using Android.App;
using Android.Content;
using IC6.Xwitter.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(LoginStore))]
namespace IC6.Xwitter.Droid
{
    public class LoginStore : ILoginStore
    {
        private enum OAuth
        {
            OAuthToken, OAuthSecret
        }

        public UserSecrets GetSecrets()
        {
            using (var prefs = Application.Context.GetSharedPreferences("IC6.Xwitter", FileCreationMode.Private))
            {
                var secrets = new UserSecrets()
                {
                    OAuthToken = prefs.GetString(OAuth.OAuthToken.ToString(), null),
                    OAuthSecret = prefs.GetString(OAuth.OAuthSecret.ToString(), null)
                };

                if (secrets.OAuthSecret == null || secrets.OAuthToken == null)
                {
                    return null;
                }

                return secrets;
            }
        }

        public void SetSecrets(UserSecrets secrets)
        {
            using (var prefs = Application.Context.GetSharedPreferences("IC6.Xwitter", FileCreationMode.Private))
            {
                using (var prefEditor = prefs.Edit())
                {
                    prefEditor.PutString(OAuth.OAuthToken.ToString(), secrets.OAuthToken);
                    prefEditor.PutString(OAuth.OAuthSecret.ToString(), secrets.OAuthSecret);
                    prefEditor.Commit();
                }
            }
        }

        public void SetSecrets(string oauthToken, string oauthSecret)
        {
            SetSecrets(new UserSecrets() { OAuthToken = oauthToken, OAuthSecret = oauthSecret });
        }
    }
}
using System.Threading.Tasks;

namespace IC6.Xwitter
{
    public interface ILoginStore
    {
        UserSecrets GetSecrets();

        Task SetSecretsAsync(UserSecrets secrets);

        Task SetSecretsAsync(string oauthToken, string oauthSecret);
    }
}
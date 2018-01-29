using System;
using System.Collections.Generic;
using System.Text;

namespace IC6.Xwitter
{
    public interface ILoginStore
    {
        void SetSecrets(UserSecrets secrets);

        void SetSecrets(string oauthToken, string oauthSecret);

        UserSecrets GetSecrets();
    }
}

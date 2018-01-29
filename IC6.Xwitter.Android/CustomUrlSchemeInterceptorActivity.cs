using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IC6.Xwitter.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    [IntentFilter(
     new[] { Intent.ActionView },
     Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
     DataSchemes = new[] { "http://com.companyname.IC6.Xwitter/callback" },
     DataPath = "oauth_token")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            //AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
        }
    }
}
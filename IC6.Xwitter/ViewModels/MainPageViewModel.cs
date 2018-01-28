using IC6.Xwitter.Models;
using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace IC6.Xwitter.ViewModels
{
    internal class MainPageViewModel : INotifyPropertyChanged
    {
        public Command _authenticateCommand;
        private IAuthorizer _auth;
        private bool _isRefreshing;
        private string _newTweetText;
        private Command _refreshTimeline, _sendTweet;
        private List<Tweet> _tweets;

        private string consumerKey = "rhYsgslO2JWW120sPCepSq6Uq";
        private string consumerSecret = "vJNFjYdCv8HO0M6mI8UTcqWdgdR9qBOEDXcxmgtV20ZjMOeZwW";

        public event PropertyChangedEventHandler PropertyChanged;

        public Command Authenticate
        {
            get
            {
                return _authenticateCommand ?? (_authenticateCommand = new Command(() => { InitAuthentication(); }));
            }
        }

        public bool IsAuthenticated
        {
            get { return _auth != null; }
        }

        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            private set
            {
                _isRefreshing = value;
                OnPropertyChanged();
            }
        }

        public string NewTweetText
        {
            get
            { return _newTweetText; }
            set
            {
                if (value.Equals(_newTweetText, StringComparison.CurrentCulture)) return;
                _newTweetText = value;
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(NewTweetText)));
                SendTweet.ChangeCanExecute();
            }
        }
        public Command RefreshTimeline
        {
            get
            {
                if (_refreshTimeline == null)
                {
                    _refreshTimeline = new Command(async () =>
                    {
                        IsRefreshing = true;

                        try
                        {
                            await RefreshAsync();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex.Message);
                        }

                        IsRefreshing = false;
                    }, () => { return IsAuthenticated; });
                }

                return _refreshTimeline;
            }
        }

        public Command SendTweet
        {
            get
            {
                if (_sendTweet == null)
                {
                    _sendTweet = new Command(
                    async () =>
                        {
                            using (var ctx = new TwitterContext(_auth))
                            {
                                await ctx.TweetAsync(NewTweetText);
                            }

                            NewTweetText = "";

                            RefreshTimeline.Execute(null);
                        },
                    () =>
                        {
                            return NewTweetText?.Length > 0 && _auth != null;
                        });
                }

                return _sendTweet;
            }
        }

        public List<Tweet> Tweets

        {
            get { return _tweets; }

            set

            {
                if (_tweets == value) return;

                _tweets = value;

                OnPropertyChanged();
            }
        }

        public void InitAuthentication()
        {
            var oauth = new Xamarin.Auth.OAuth1Authenticator(consumerKey, consumerSecret,
                 new Uri("https://api.twitter.com/oauth/request_token"),
                  new Uri("https://api.twitter.com/oauth/authorize"),
                  new Uri("https://api.twitter.com/oauth/access_token"),
                  new Uri("http://127.0.0.1"));

            oauth.Completed += Oauth_Completed_GetAuthorizer;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(oauth);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)

        {
            if (propertyName == null)

                throw new ArgumentNullException("Can't call OnPropertyChanged with a null property name.", propertyName);

            PropertyChangedEventHandler propChangedHandler = PropertyChanged;

            if (propChangedHandler != null)

                propChangedHandler(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Oauth_Completed_GetAuthorizer(object sender, Xamarin.Auth.AuthenticatorCompletedEventArgs e)
        {
            try
            {
                var authSvc = DependencyService.Get<ILinqToTwitterAuthorizer>();

                _auth = authSvc.GetAuthorizer(consumerKey,
                    consumerSecret,
                    e.Account.Properties["oauth_token"],
                    e.Account.Properties["oauth_token_secret"]);

                RefreshTimeline.ChangeCanExecute();

                RefreshTimeline.Execute(null);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private async System.Threading.Tasks.Task RefreshAsync()
        {
            await _auth.AuthorizeAsync();

            using (var ctx = new TwitterContext(_auth))
            {
                var srch = await
                      (from tweet in ctx.Status
                       where tweet.Type == StatusType.Home
                       select new Tweet()
                       {
                           StatusID = tweet.StatusID,
                           ScreenName = tweet.User.ScreenNameResponse,
                           Text = tweet.Text,
                           ImageUrl = tweet.RetweetedStatus != null && tweet.RetweetedStatus.User != null ?
                                      tweet.RetweetedStatus.User.ProfileImageUrl.Replace("http://", "https://") : null
                       }).ToListAsync();

                Tweets = new List<Tweet>(srch);
            }
        }
    }
}
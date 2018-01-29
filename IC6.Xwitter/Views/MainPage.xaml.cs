using IC6.Xwitter.ViewModels;
using Xamarin.Forms;

namespace IC6.Xwitter.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel(DependencyService.Get<ILoginStore>(), DependencyService.Get<ILinqToTwitterAuthorizer>());
        }

        private MainPageViewModel ViewModel
        {
            get { return BindingContext as MainPageViewModel; }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel == null)
                return;

            if (!ViewModel.IsAuthenticated)
            {
                ViewModel.Authenticate.Execute(null);
            }
            else
            {
                ViewModel.RefreshTimeline.Execute(null);
            }
        }
    }
}
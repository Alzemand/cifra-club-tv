using AppStudio.Services;
using AppStudio.ViewModels;

using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppStudio.Views
{
    public sealed partial class CifraClubNewsDetail : Page
    {
        private NavigationHelper _navigationHelper;

        private DataTransferManager _dataTransferManager;

        public CifraClubNewsDetail()
        {
            this.InitializeComponent();
            _navigationHelper = new NavigationHelper(this);

            CifraClubNewsModel = new CifraClubNewsViewModel();
        }

        public CifraClubNewsViewModel CifraClubNewsModel { get; private set; }

        public NavigationHelper NavigationHelper
        {
            get { return _navigationHelper; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;

            _navigationHelper.OnNavigatedTo(e);

            if (CifraClubNewsModel != null)
            {
                await CifraClubNewsModel.LoadItemsAsync();
                CifraClubNewsModel.SelectItem(e.Parameter);

                CifraClubNewsModel.ViewType = ViewTypes.Detail;
            }
            DataContext = this;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigationHelper.OnNavigatedFrom(e);
            _dataTransferManager.DataRequested -= OnDataRequested;
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            if (CifraClubNewsModel != null)
            {
                CifraClubNewsModel.GetShareContent(args.Request);
            }
        }
    }
}

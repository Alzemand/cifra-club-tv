using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Net.NetworkInformation;

using Windows.UI.Xaml;

using AppStudio.Services;
using AppStudio.Data;

namespace AppStudio.ViewModels
{
    public class MainViewModel : BindableBase
    {
       private CifraClubNewsViewModel _cifraClubNewsModel;
       private VideosViewModel _videosModel;
       private CifraClubTVViewModel _cifraClubTVModel;
        private PrivacyViewModel _privacyModel;

        private ViewModelBase _selectedItem = null;

        public MainViewModel()
        {
            _selectedItem = CifraClubNewsModel;
            _privacyModel = new PrivacyViewModel();

        }
 
        public CifraClubNewsViewModel CifraClubNewsModel
        {
            get { return _cifraClubNewsModel ?? (_cifraClubNewsModel = new CifraClubNewsViewModel()); }
        }
 
        public VideosViewModel VideosModel
        {
            get { return _videosModel ?? (_videosModel = new VideosViewModel()); }
        }
 
        public CifraClubTVViewModel CifraClubTVModel
        {
            get { return _cifraClubTVModel ?? (_cifraClubTVModel = new CifraClubTVViewModel()); }
        }

        public void SetViewType(ViewTypes viewType)
        {
            CifraClubNewsModel.ViewType = viewType;
            VideosModel.ViewType = viewType;
            CifraClubTVModel.ViewType = viewType;
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                UpdateAppBar();
            }
        }

        public Visibility AppBarVisibility
        {
            get
            {
                return SelectedItem == null ? AboutVisibility : SelectedItem.AppBarVisibility;
            }
        }

        public Visibility AboutVisibility
        {

         get { return Visibility.Visible; }
        }

        public void UpdateAppBar()
        {
            OnPropertyChanged("AppBarVisibility");
            OnPropertyChanged("AboutVisibility");
        }

        /// <summary>
        /// Load ViewModel items asynchronous
        /// </summary>
        public async Task LoadDataAsync(bool forceRefresh = false)
        {
            var loadTasks = new Task[]
            { 
                CifraClubNewsModel.LoadItemsAsync(forceRefresh),
                VideosModel.LoadItemsAsync(forceRefresh),
                CifraClubTVModel.LoadItemsAsync(forceRefresh),
            };
            await Task.WhenAll(loadTasks);
        }

        //
        //  ViewModel command implementation
        //
        public ICommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    await LoadDataAsync(true);
                });
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateToPage("AboutThisAppPage");
                });
            }
        }

        public ICommand PrivacyCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationServices.NavigateTo(_privacyModel.Url);
                });
            }
        }
    }
}

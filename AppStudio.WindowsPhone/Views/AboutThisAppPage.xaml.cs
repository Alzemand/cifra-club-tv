using System;

using Windows.UI.Xaml.Controls;

using AppStudio.ViewModels;

namespace AppStudio.Views
{
    public sealed partial class AboutThisAppPage : Page
    {
        public AboutThisAppPage()
        {
            this.InitializeComponent();
            AboutModel = new AboutThisAppViewModel();
            this.DataContext = this;
        }

        public AboutThisAppViewModel AboutModel { get; private set; }

        private void Twitter_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Uri targetUri = new Uri(@"https://twitter.com/alzemand");
        }
    }
}

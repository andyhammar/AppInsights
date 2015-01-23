using System;
using System.Collections.Generic;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Xaml.Navigation;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace AppInsightsWinApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly TelemetryClient _telemetryClient;

        public MainPage()
        {
            _telemetryClient = new TelemetryClient();
            this.InitializeComponent();

        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await FetchPageToTestFiddler();

            base.OnNavigatedTo(e);
        }

        private static async Task FetchPageToTestFiddler()
        {
            try
            {
                var client = new HttpClient();
                var html = await client.GetStringAsync("http://m.di.se");
            }
            catch (Exception exception)
            {
                //ignore
            }
        }

        private void Go_OnClick(object sender, RoutedEventArgs e)
        {
            _telemetryClient.TrackEvent("GoButtonClicked");

            var profile = NetworkInformation.GetInternetConnectionProfile();
            var level = profile.GetNetworkConnectivityLevel();
            if (level == NetworkConnectivityLevel.None ||
                level == NetworkConnectivityLevel.LocalAccess)
            {
                _telemetryClient.TrackEvent("GoButtonClicked-WhenOffline");
            }

        }

        private void GoToNextPage_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (DetailsPage));
        }

        private void ThrowException_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                throw new ActionNotSupportedException("custom message for app insights",
                    new Exception("inner exception for appinsights"));
            }
            catch (Exception exc)
            {
                _telemetryClient.TrackException(exc);
                throw;
            }
        }

        private void SameSession_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) { return; }
            
            var text = button.Content ?? string.Empty;

            var profile = NetworkInformation.GetInternetConnectionProfile();
            var level = NetworkConnectivityLevel.None;
            try
            {
                level = profile.GetNetworkConnectivityLevel();
            }
            catch (Exception exception) { }

            _telemetryClient.TrackEvent(string.Format("[{0}][{1}] {2}", level, OfflineTextBox.Text, text));
        }
    }
}

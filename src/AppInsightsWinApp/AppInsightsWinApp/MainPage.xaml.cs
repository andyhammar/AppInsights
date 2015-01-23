using System;
using System.ServiceModel;
using Windows.Networking.Connectivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Microsoft.ApplicationInsights;

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
            var level = profile.GetNetworkConnectivityLevel();

            _telemetryClient.TrackEvent(string.Format("[conn-level: {0}] {1}", level, text));
        }
    }
}

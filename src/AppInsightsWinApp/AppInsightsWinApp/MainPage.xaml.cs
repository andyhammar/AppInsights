using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Windows.UI.Xaml.Media;
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

        private async void Go_OnClick(object sender, RoutedEventArgs e)
        {

            try
            {
                _telemetryClient.TrackEvent("GoButtonClicked");

                var waitTime = GetRandom(0, 100);
                if (waitTime > 50)
                    throw new Exception("time too long");

                var waitDiff = await WaitAndGetDiffAsync(waitTime);

                var properties = 
                    new Dictionary<string, string>{{"WaitTimeExpected", waitTime.ToString()}};

                _telemetryClient.TrackMetric("WaitDiffValueAsMetric", waitDiff, properties);

                _telemetryClient.TrackEvent("WaitDiffEvent", properties,
                    new Dictionary<string, double>{{"WaitDiffValueAsEvent", waitDiff}});
            }
            catch (Exception exception)
            {
                _telemetryClient.TrackException(exception);
            }


            var profile = NetworkInformation.GetInternetConnectionProfile();
            var level = profile.GetNetworkConnectivityLevel();
            if (level == NetworkConnectivityLevel.None ||
                level == NetworkConnectivityLevel.LocalAccess)
            {
                _telemetryClient.TrackEvent("GoButtonClicked-WhenOffline");
            }

        }

        private static async Task<long> WaitAndGetDiffAsync(int waitTime)
        {
            var stopWatch = Stopwatch.StartNew();
            await Task.Delay(waitTime);
            var actualTime = stopWatch.ElapsedMilliseconds;
            return actualTime - waitTime;
        }

        private static int GetRandom(int minValue, int maxValue)
        {
            return new Random((int)DateTime.Now.Ticks).Next(minValue, maxValue);
        }

        private void GoToNextPage_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(DetailsPage));
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

        private async void EnforeSessionButton_OnClick(object sender, RoutedEventArgs e)
        {
            var sessionId = _telemetryClient.Context.Session.Id;
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aiSession", new[] { sessionId });
            var response = await client.GetStringAsync("http://localhost:52813/api/values");

            _telemetryClient.TrackEvent("after header send");
        }

        private void PollButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (PollPage));
        }

        private async void RunHeavy_OnClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var color = button.Background;
            button.Background = new SolidColorBrush(Colors.Orange);

            _telemetryClient.TrackEvent("HeavyOp-Starting");
            await Task.Run(() =>
            {
                var sum = 0;
                for (int i = 0; i < 1000*1000; i++)
                {
                    sum += new Random(i).Next(0,100);
                    //Debug.WriteLine(string.Format("sum: {0}", sum));
                }
                Debug.WriteLine("done summing");
            });

            _telemetryClient.TrackEvent("HeavyOp-Done");

            Debug.WriteLine("done");
            button.Background = color;
        }
    }
}

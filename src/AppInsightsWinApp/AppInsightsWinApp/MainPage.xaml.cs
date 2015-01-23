using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
//using Microsoft.ApplicationInsights;

namespace AppInsightsWinApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private readonly TelemetryClient _telemetryClient;

        public MainPage()
        {
            //_telemetryClient = new TelemetryClient();
            this.InitializeComponent();
        }

        private void Go_OnClick(object sender, RoutedEventArgs e)
        {
            //_telemetryClient.TrackEvent("GoButtonClicked");
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
                //_telemetryClient.TrackException(exc);
                throw;
            }
        }
    }
}

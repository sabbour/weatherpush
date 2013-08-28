using Microsoft.WindowsAzure.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.PushNotifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace WeatherApp
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : WeatherApp.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private async void registerButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the current push notification channel
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var hub = new NotificationHub("myhub", "Endpoint=sb://sabbour.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=GgJk6ED18JfKc4Ch3dmAtoXcmbZXxUdl4fzefq5XyLU=");
        
            // Get the selected cityname
            var cityName = citiesCombobox.SelectedValue as string;

            // Construct the template the app is expecting
            var temperatureProperty = celsiusSwitch.IsOn ? "tempC" : "tempF";
            var temperatureString = celsiusSwitch.IsOn ? "Celsius" : "Fahrenheit";
            var message = "Temperature in " + cityName + " is $(" + temperatureProperty + ") " + temperatureString;

            // Append the message into the toast template
            string toastTemplate = @"<toast><visual><binding template=""ToastText01""><text id=""1"">" + message + "</text></binding></visual></toast>";
                   
            // Register with the notification hub, and pass the cityname as a tag you are interested in as well as the template
            await hub.RegisterTemplateAsync(channel.Uri, toastTemplate, "TemperatureToastTemplate", new string[] { cityName });
        }
    }
}

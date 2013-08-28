using Microsoft.ServiceBus.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServer
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the hub
            var hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://sabbour.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=qqlk0PPmc0RAv/HgjMOAED3zAWQ+UqAGgupsn3JFHXM=", "myhub");

            // Dumb "database" of temperatures
            var citiesDB = new Dictionary<string, string>
            {
                {"Cairo", "35,95"},                
                {"Lagos", "31,88"},
                {"Nairobi", "21,70"}
            };

            // Loop over the cities and push notifications to interested people
            foreach (var city in citiesDB)
            {
                var cityName = city.Key;
                var temperatures = city.Value.Split(',');
                var msg = new Dictionary<string, string> { 
                    {"tempC", temperatures[0]},
                    {"tempF", temperatures[1]}
                };

                // Push the notification. Note that we are using "SendTemplateNotification" and not the specific SendWNS, SendMPNS, ...
                hub.SendTemplateNotificationAsync(msg, cityName);
            }

            Console.ReadLine();

        }
    }
}

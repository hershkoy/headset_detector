using NAudio.CoreAudioApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using WebSocketSharp.Server;

namespace Headphone_tracker
{
    class Program
    {
        static WebSocketServer wss;
        static void Main(string[] args)
        {
            var enumerator = new MMDeviceEnumerator();

            // Allows you to enumerate rendering devices in certain states
            var endpoints = enumerator.EnumerateAudioEndPoints(
                DataFlow.Render,
                DeviceState.Unplugged | DeviceState.Active);
            foreach (var endpoint in endpoints)
            {
                Console.WriteLine("{0} - {1}", endpoint.DeviceFriendlyName, endpoint.State);
            }

            // Aswell as hook to the actual event
            var nc = new NotificationClient();
            nc.DeviceConnected += Nc_DeviceConnected;
            nc.DeviceRemoved += Nc_DeviceRemoved;
            enumerator.RegisterEndpointNotificationCallback(nc);
            wss = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 5000);
            wss.AddWebSocketService<WebSocketListener>("/Headset");
            wss.Start();
            Console.WriteLine($"I'm listening {wss.Address}:{wss.Port}/Headset");
            Console.ReadKey(true);
        }

        private static void Nc_DeviceRemoved(object sender, string e)
        {
            wss.WebSocketServices.Broadcast("Device removed: " + e);
            Console.WriteLine("Device removed notification sent!");
        }

        private static void Nc_DeviceConnected(object sender, string e)
        {
            wss.WebSocketServices.Broadcast("Device connected: " + e);
            Console.WriteLine("Device connected notification sent!");
        }
    }
}

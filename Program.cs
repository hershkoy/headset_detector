

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using CSCore.CoreAudioAPI;

using WebSocketSharp.Server;

namespace Headphone_tracker
{
    class Program
    {
        static MMDeviceEnumerator enummerator = new MMDeviceEnumerator();
        static WebSocketServer wss;
        static void Main(string[] args)
        {
            wss = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 5000);
            wss.AddWebSocketService<WebSocketListener>("/Headset");
            wss.Start();
            Console.WriteLine($"I'm listening {wss.Address}:{wss.Port}/Headset");
            enummerator.DeviceStateChanged += (s, e) =>
            {
                switch (e.DeviceState)
                {
                    case DeviceState.Active:
                        wss.WebSocketServices.Broadcast("Device connected: " + e.DeviceId);
                        Console.WriteLine("Device connected notification sent!");
                        break;
                    case DeviceState.NotPresent:
                    case DeviceState.Disabled:
                    case DeviceState.UnPlugged:
                        wss.WebSocketServices.Broadcast("Device removed: " + e.DeviceId);
                        Console.WriteLine("Device removed notification sent!");
                        break;
                    case DeviceState.All:
                        break;
                    default:
                        break;
                }
            };

            Console.ReadKey(true);
        }

    }
}

using AudioDeviceUtil;

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
        static AudioDeviceManager audioDeviceSwitcher = new AudioDeviceManager();
        static void Main(string[] args)
        {
            audioDeviceSwitcher.RegisterForNotification = true;
            audioDeviceSwitcher.AudioDeviceEvent += AudioDeviceSwitcher_AudioDeviceEvent;
            wss = new WebSocketServer(IPAddress.Parse("127.0.0.1"), 5000);
            wss.AddWebSocketService<WebSocketListener>("/Headset");
            wss.Start();
            Console.WriteLine($"I'm listening {wss.Address}:{wss.Port}/Headset");
            Console.ReadKey(true);
        }

        private static void AudioDeviceSwitcher_AudioDeviceEvent(object sender, AudioDeviceNotificationEventArgs e)
        {
            switch (e.State)
            {
                case AudioDeviceStateType.Active:
                    wss.WebSocketServices.Broadcast("Device connected: " + e.DeviceId);
                    Console.WriteLine("Device connected notification sent!");
                    break;
                case AudioDeviceStateType.Disabled:
                    break;
                case AudioDeviceStateType.NotPresent:
                    break;
                case AudioDeviceStateType.Unplugged:
                    wss.WebSocketServices.Broadcast("Device removed: " + e.DeviceId);
                    Console.WriteLine("Device removed notification sent!");
                    break;
                case AudioDeviceStateType.StateMaskAll:
                    break;
                default:
                    break;
            }
        }

    }
}

using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

using System;

namespace Headphone_tracker
{
    internal class NotificationClient : IMMNotificationClient
    {
        public event EventHandler<string> DeviceConnected, DeviceRemoved;
        public void OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            switch (newState)
            {
                case DeviceState.Active:
                    DeviceConnected?.Invoke(this, deviceId);
                    break;
                case DeviceState.Disabled:
                    break;
                case DeviceState.NotPresent:
                    break;
                case DeviceState.Unplugged:
                    DeviceRemoved?.Invoke(this, deviceId);
                    break;
                case DeviceState.All:
                    break;
                default:
                    break;
            }
            Console.WriteLine("OnDeviceStateChanged\n Device Id -->{0} : Device State {1}", deviceId, newState);
        }

        public void OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
        }

        public void OnPropertyValueChanged(string pwstrDeviceId, PropertyKey key)
        {
        }

        public void OnDeviceAdded(string pwstrDeviceId)
        {
        }

        public void OnDeviceRemoved(string deviceId)
        {
        }
    }
}
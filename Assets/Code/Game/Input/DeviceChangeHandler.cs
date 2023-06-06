using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assembly.IBX.Main
{
    internal static class DeviceChangeHandler
    {
        /// <summary>
        /// Logs a device change and shows a popup
        /// </summary>
        /// <param name="device"></param>
        /// <param name="change"></param>
        internal static void DeviceConnected(InputDevice device, InputDeviceChange change)
        {
            if(change == InputDeviceChange.Reconnected)
            {
                Debug.Log($"Device Reconnected {device.displayName}");
            }
            else
            {
                Debug.Log($"Device Added {device.displayName}");
            }
        }

        /// <summary>
        /// Logs a device change and shows a popup
        /// </summary>
        /// <param name="device"></param>
        internal static void DeviceDisconnected(InputDevice device)
        {
            Debug.Log($"Device Disconnected {device.displayName}");
        }
    }
}

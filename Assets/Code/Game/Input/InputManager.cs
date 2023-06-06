using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Utilities;

namespace Assembly.IBX.Main.Input
{
    public static class InputManager
    {
        public static Controls controls { get; private set; }

        internal static bool enableDualshockLightEffects = false;

        /// <summary>
        /// Initializer
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InputManagerInitialize()
        {
            controls = new Controls();
            controls.Enable();

            InputSystem.onDeviceChange += OnDeviceChange;
        }

        /// <summary>
        /// Get the action mappings for the game
        /// </summary>
        /// <returns>An input action map</returns>
        public static Controls.GameActions GetGameActions()
        {
            return controls.Game;
        }

        /// <summary>
        /// Get the action mappings for the UI
        /// </summary>
        /// <returns>An input action map</returns>
        public static Controls.UIActions GetUIActions()
        {
            return controls.UI;
        }

        /// <summary>
        /// This method is executed whenever a device change event is fired by unity
        /// </summary>
        /// <param name="device">The device changed</param>
        /// <param name="change">The change this device underwent</param>
        private static void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch(change)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                    DeviceChangeHandler.DeviceConnected(device, change);
                    break;

                case InputDeviceChange.Disconnected:
                    DeviceChangeHandler.DeviceDisconnected(device);
                    break;
            }
        }
    }
}

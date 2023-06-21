using System;
using Unity.Services.Core;
using UnityEngine;

namespace Assembly.IBX.Main
{
    public static class Initialize
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static async void UnityServicesInit()
        {
            Debug.Log("Subsystem Registration");

            if(Application.internetReachability != NetworkReachability.NotReachable)
            {
                await UnityServices.InitializeAsync();

                if(UnityServices.State == ServicesInitializationState.Initialized)
                {
                    Debug.Log("Unity Services Initialized!");
                }
            }
            else
            {
                Debug.LogWarning("Unity Services could not be initialized due to no internet. The game is still playable, but many features will not work properly.");
            }
        }
    }
}

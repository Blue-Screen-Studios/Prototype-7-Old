using UnityEngine;

namespace Assembly.IBX.Main.UtilityComponents
{
    internal class KeepAlive : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}

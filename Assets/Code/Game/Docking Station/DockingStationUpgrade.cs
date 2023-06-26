using UnityEngine;

namespace Assembly.IBX.Main.Station
{
    [CreateAssetMenu(fileName = "Docking Station Upgrade", menuName = "ScriptableObjects/Docking Station Upgrade")]
    public class DockingStationUpgrade : ScriptableObject
    {
        public enum UpgradeType { BATTERY, DEFENSE_WEAPON, HEALTH, RESISTANCE };


        [Tooltip("What type of upgrade are we dealing with? This is required so we can update the overall station parameters.")]
        [SerializeField] internal UpgradeType type;

        [Tooltip("An array of world objects that are appropriate for this upgrade. One world object will be chosen at random to be placed on the docking station.")]
        [SerializeField] internal GameObject[] availibleWorldObjects;

        [Header("A field for determining amount. EX: Force Field strength, Health Boost, Battery Charge")]
        [SerializeField] internal int amount;
    }
}

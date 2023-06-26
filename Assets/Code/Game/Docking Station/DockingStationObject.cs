using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assembly.IBX.Main.Station
{
    public class DockingStationObject
    {
        /// <summary>
        /// Does this docking station have a player attatched?
        /// </summary>
        public bool HasPlayer { get; private set; }

        /// <summary>
        /// A list of applied upgrades
        /// </summary>
        public AppliedUpgrades UpgradesApplied { get; private set; }

        /// <summary>
        /// A data structure containing upgrades
        /// </summary>
        public struct AppliedUpgrades
        {
            public bool FrontLeviNora;
            public bool ReartLeviNora;

            public bool SecondLevelBuilt;

            public List<DockingStationUpgrade> lowerUpgradeSlots;
            public List<DockingStationUpgrade> upperUpgradeSlots;
            public List<DockingStationUpgrade> extendedUpgradeSlots;
        }

        /// <summary>
        /// Constructs a new docking station with zero upgrades
        /// </summary>
        public DockingStationObject()
        {
            this.HasPlayer = false;

            this.UpgradesApplied = new AppliedUpgrades
            {
                FrontLeviNora = false,
                ReartLeviNora = false,

                SecondLevelBuilt = false,

                lowerUpgradeSlots = new List<DockingStationUpgrade>(),
                upperUpgradeSlots = new List<DockingStationUpgrade>(),
                extendedUpgradeSlots = new List<DockingStationUpgrade>()
            };
        }

        /// <summary>
        /// Connects a player to this docking station
        /// </summary>
        public void ConnectPlayer()
        {
            this.HasPlayer = true;
        }

        /// <summary>
        /// Disconnects a player from this docking station
        /// </summary>
        public void DisconnectPlayer()
        {
            this.HasPlayer = false;
        }

        /// <summary>
        /// Adds a LeviNora to the docking station
        /// </summary>
        /// <returns>True if a levinora can be added</returns>
        public bool AddLeviNora()
        {
            if(UpgradesApplied.ReartLeviNora && UpgradesApplied.FrontLeviNora)
            {
                return false;
            }
            else
            {
                if(UpgradesApplied.ReartLeviNora)
                {
                    AppliedUpgrades temp = UpgradesApplied;
                    temp.FrontLeviNora = true;

                    UpgradesApplied = temp;

                    return true;
                }
                else
                {
                    AppliedUpgrades temp = UpgradesApplied;
                    temp.ReartLeviNora = true;
                    
                    UpgradesApplied = temp;

                    return true;
                }
            }
        }

        /// <summary>
        /// Builds a second level to the docking station
        /// </summary>
        /// <returns>True if this upgrade is availible</returns>
        public bool BuildSecondLevel()
        {
            AppliedUpgrades temp = UpgradesApplied;
            temp.SecondLevelBuilt = true;

            UpgradesApplied = temp;

            return !UpgradesApplied.SecondLevelBuilt;
        }

    }
}

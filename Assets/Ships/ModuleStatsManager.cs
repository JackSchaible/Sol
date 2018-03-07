using UnityEngine;

namespace Assets.Ships
{
    public class ModuleStatsManager
    {
        public ModuleStatsManager()
        {
        }

        public ModuleStats[] Get()
        {
            return Generate();
        }

        private ModuleStats[] Generate()
        {
            return new ModuleStats[]
            {
                new CommandModuleStats(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Basic Cockpit - Build", Vector3.one, new Vector2(0.5f, 0.5f),
                    "Basic Cockpit", "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                    50, 31, 0, 0, 0, 0, 25, 500, 25, 1, 200, 8),
                new CommandModuleStats(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Cockpit - Build", Vector3.one, new Vector2(0.5f, 0.5f),
                    "Advanced Cockpit", "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.",
                    50, 31, 0, 0, 0, 0, 25, 550, 35, 1, 500, 12),
                new CommandModuleStats(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build", Vector3.one, new Vector2(0.5f, 0.5f),
                    "Heavy Cockpit", "A basic cockpit with a bit more space for 2 pilots.",
                    50, 50, 0, 0, 0, 0, 40, 750, 30, 2, 250, 16),
                new CommandModuleStats(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build", Vector3.one, new Vector2(0.5f, 0.5f),
                    "Advanced Heavy Cockpit", "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 50, 0, 0, 0, 0, 50, 800, 50, 2, 550, 24),
            };
        }
    }
}

using Assets.Ships.Modules;
using UnityEngine;

namespace Assets.Ships
{
    public class ModuleBlueprintsManager
    {
        public ModuleBlueprintsManager()
        {
        }

        public ModuleBlueprints[] Get()
        {
            return Generate();
        }

        private ModuleBlueprints[] Generate()
        {
            return new ModuleBlueprints[]
            {
                new CockpitModuleBlueprints(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Basic Cockpit - Build", "Basic Cockpit",
                    "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                    25, 500, 25, new []{ ConnectorPositions.Backward }, 1, 200, 1, 8), 
                new CockpitModuleBlueprints(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Cockpit - Build", "Advanced Cockpit", 
                    "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.",
                    25, 550, 35, new []{ ConnectorPositions.Backward }, 1, 500, 1, 12),
                new CockpitModuleBlueprints(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build", "Heavy Cockpit",
                    "A basic cockpit with a bit more space for 2 pilots.",
                    40, 750, 30, new []{ ConnectorPositions.Backward }, 2, 250, 2, 16),
                new CockpitModuleBlueprints(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build", "Advanced Heavy Cockpit",
                    "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 800, 50, new []{ ConnectorPositions.Backward }, 2, 550, 2, 24),
                new CockpitModuleBlueprints(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Tactical Cockpit - Build", "Tactical Cockpit",
                    "A much larger cockpit, usually reserved for long-range strike craft, scout craft, bombers, or anything else that requires someone other than a pilot. Has space for 2 pilots and 1 technical officer. Includes a more advanced AI for helping with technical tasks, as well as maintaining life support and doing navigation calculations, among other things.",
                    75, 1500, 100, new []{ ConnectorPositions.Backward }, 3, 1000, 3, 36),
            };
        }
    }
}

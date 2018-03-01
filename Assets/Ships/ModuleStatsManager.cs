using Assets.Data;

namespace Assets.Ships
{
    public class ModuleStatsManager
    {
        private IDataManager<ModuleStats[]> _dataManager;

        public ModuleStatsManager()
        {
            //_dataManager = new XmlDataManager<ModuleStats[]>();
        }

        public ModuleStats[] Get()
        {
            return Generate();
        }

        private ModuleStats[] Generate()
        {
            return new ModuleStats[]
            {
                new CommandModuleStats(ControlCentreTypes.SmallShip, "Ships/Control Centres/Small Ship/Basic Cockpit - Build", "Basic Cockpit", "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.", 50, 31, 0, 0, 0, 0, 25, 3, 500, 25, 1, 200, 8),
                new CommandModuleStats(ControlCentreTypes.SmallShip, "Ships/Control Centres/Small Ship/Advanced Cockpit - Build", "Advanced Cockpit", "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.", 50, 31, 0, 0, 0, 0, 25, 3, 550, 35, 1, 500, 12),
            };
        }
    }
}

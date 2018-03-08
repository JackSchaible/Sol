using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CockpitModuleBlueprints : CommandModuleBlueprints
    {
        public int PersonnelHoused { get; set; }

        public CockpitModuleBlueprints()
        {
            
        }

        public CockpitModuleBlueprints(string moduleType, string buildSprite, string name, string description,
            int health, int weight, int cost, ConnectorPositions[] connectors, int crewRequirement, int powerConumption,
            int personnelHoused, int commandSupplied)
            : base(moduleType, buildSprite, name, description, health, weight, cost, connectors, crewRequirement,
                  powerConumption, commandSupplied)
        {
            PersonnelHoused = personnelHoused;
        }
    }
}

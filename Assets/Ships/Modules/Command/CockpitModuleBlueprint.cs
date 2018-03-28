using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CockpitModuleBlueprint : CommandModuleBlueprint
    {
        public int PersonnelHoused { get; set; }

        public CockpitModuleBlueprint()
        {
            
        }

        public CockpitModuleBlueprint(string moduleType, string buildSprite, string name, string description,
            int health, int weight, int cost, ConnectorPosition[] connectors, int crewRequirement, int powerConumption,
            int personnelHoused, int commandSupplied)
            : base(moduleType, buildSprite, name, description, health, weight, cost, connectors,
                  new []{ Modules.ExclusionVectors.PlaneAndAbove }, crewRequirement, powerConumption, commandSupplied)
        {
            PersonnelHoused = personnelHoused;
        }
    }
}

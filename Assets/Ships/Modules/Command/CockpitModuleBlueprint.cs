using Assets.Common.Utils;
using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CockpitModuleBlueprint : CommandModuleBlueprint
    {
        public int PersonnelHoused { get; set; }

        public CockpitModuleBlueprint()
        {
            
        }

        public CockpitModuleBlueprint(string moduleType, string buildSprite, IntVector[] space, string name,
            string description, int health, float weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors, int crewRequirement,
            float powerConumption, int personnelHoused, int commandSupplied)
            : base(moduleType, buildSprite, space, name, description, health, weight, cost, areConnectorsMandatory, connectors,
                  new []{ new ExclusionVector(new [] {ExclusionVectorDirections.PlaneAndForward}) }, crewRequirement, powerConumption, commandSupplied)
        {
            PersonnelHoused = personnelHoused;
        }
    }
}

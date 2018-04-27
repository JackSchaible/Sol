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

        public override ModuleBlueprint Copy()
        {
            var connectors = new ConnectorPosition[Connectors.Length];
            var space = new IntVector[Space.Length];

            Connectors.CopyTo(connectors, 0);
            Space.CopyTo(space, 0);

            return new CockpitModuleBlueprint(
                ModuleSubtype, BuildSprite, space, Name, Description, Health, Weight, Cost, AreConnectorsMandatory,
                connectors, CrewRequirement, PowerConumption, PersonnelHoused, CommandSupplied);
        }
    }
}

using UnityEngine;

namespace Assets.Ships.Modules.Command
{
    public class CockpitModuleBlueprint : CommandModuleBlueprint
    {
        public int PersonnelHoused { get; set; }

        public CockpitModuleBlueprint()
        {
            
        }

        public CockpitModuleBlueprint(string moduleType, string buildSprite, string[,,] componentSprites, Vector3Int[] space, string name, string description, int health, float weight, Cost cost, bool areConnectorsMandatory, Connector[] connectors, int crewRequirement, float powerConumption, int personnelHoused, int commandSupplied)
            : base(moduleType, buildSprite, componentSprites, space, name, description, health, weight, cost, areConnectorsMandatory, connectors, new []{ new ExclusionVector(new [] {ExclusionVectorDirections.PlaneAndForward}) }, crewRequirement, powerConumption, commandSupplied)
        {
            PersonnelHoused = personnelHoused;
        }

        public override ModuleBlueprint Copy()
        {
            var connectors = new Connector[Connectors.Length];
            var space = new Vector3Int[Space.Length];

            Connectors.CopyTo(connectors, 0);
            Space.CopyTo(space, 0);

            return new CockpitModuleBlueprint(ModuleSubtype, BuildSprite, ComponentSprites, space, Name, Description, Health, Weight, Cost, AreConnectorsMandatory, connectors, CrewRequirement, PowerConumption, PersonnelHoused, CommandSupplied);
        }
    }
}

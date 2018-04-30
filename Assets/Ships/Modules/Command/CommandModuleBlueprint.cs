using Assets.Ships.Modules;
using UnityEngine;

namespace Assets.Ships
{
    public class CommandModuleBlueprint : ModuleBlueprint
    {
        public int CommandSupplied { get; set; }
        public override string[] RelatedAbilities { get { return new[] { "Charisma", "Intelligence" }; } }

        public CommandModuleBlueprint()
        {
            
        }

        public CommandModuleBlueprint(string moduleType, string buildSprite, Vector3Int[] space, string name,
            string description, int health, float weight, Cost cost, bool areConnectorsMandatory, Connector[] connectors,
            ExclusionVector[] exclusionVectors, int crewRequirement, float powerConumption, int commandSupplied)
            : base(ModuleTypes.ControlCentre, moduleType, buildSprite, space, name, description, health, weight, cost,
                areConnectorsMandatory, connectors, exclusionVectors, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }

        public override ModuleBlueprint Copy()
        {
            var connectors = new Connector[Connectors.Length];
            var exclusionVectors = new ExclusionVector[ExclusionVectors.Length];
            var space = new Vector3Int[Space.Length];

            Connectors.CopyTo(connectors, 0);
            ExclusionVectors.CopyTo(exclusionVectors, 0);
            Space.CopyTo(space, 0);

            return new CommandModuleBlueprint(
                ModuleSubtype, BuildSprite, space, Name, Description, Health, Weight, Cost, AreConnectorsMandatory,
                connectors, exclusionVectors, CrewRequirement, PowerConumption, CommandSupplied);
        }
    }
}

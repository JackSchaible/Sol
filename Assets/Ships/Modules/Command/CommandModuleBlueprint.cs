using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CommandModuleBlueprint : ModuleBlueprint
    {
        public int CommandSupplied { get; set; }
        public override string[] RelatedAbilities { get { return new[] { "Charisma", "Intelligence" }; } }

        public CommandModuleBlueprint()
        {
            
        }

        public CommandModuleBlueprint(string moduleType, string buildSprite, List<IntVector> space, string name,
            string description, int health, int weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors,
            ExclusionVector[] exclusionVectors, int crewRequirement, int powerConumption, int commandSupplied)
            : base(ModuleTypes.ControlCentre, moduleType, buildSprite, space, name, description, health, weight, cost,
                areConnectorsMandatory, connectors, exclusionVectors, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }
    }
}

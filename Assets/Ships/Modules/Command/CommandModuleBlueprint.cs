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

        public CommandModuleBlueprint(string moduleType, string buildSprite, IntVector[] space, string name,
            string description, int health, float weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors,
            ExclusionVector[] exclusionVectors, int crewRequirement, float powerConumption, int commandSupplied)
            : base(ModuleTypes.ControlCentre, moduleType, buildSprite, space, name, description, health, weight, cost,
                areConnectorsMandatory, connectors, exclusionVectors, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }

        public override ModuleBlueprint Copy()
        {
            var connectors = new ConnectorPosition[Connectors.Length];
            var exclusionVectors = new ExclusionVector[ExclusionVectors.Length];
            var space = new IntVector[Space.Length];

            Connectors.CopyTo(connectors, 0);
            ExclusionVectors.CopyTo(exclusionVectors, 0);
            Space.CopyTo(space, 0);

            return new CommandModuleBlueprint(
                ModuleSubtype, BuildSprite, space, Name, Description, Health, Weight, Cost, AreConnectorsMandatory,
                connectors, exclusionVectors, CrewRequirement, PowerConumption, CommandSupplied);
        }
    }
}

using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CommandModuleBlueprints : ModuleBlueprints
    {
        public int CommandSupplied { get; set; }
        public override string[] RelatedAbilities { get { return new[] { "Charisma", "Intelligence" }; } }

        public CommandModuleBlueprints()
        {
            
        }

        public CommandModuleBlueprints(string moduleType, string buildSprite, string name, string description, 
            int health, int weight, int cost, ConnectorPosition[] connectors, ExclusionVectors[] exclusionVectors,
            int crewRequirement, int powerConumption, int commandSupplied)
            : base(ModuleTypes.ControlCentre, moduleType, buildSprite, name, description, health, weight, cost,
                connectors, exclusionVectors, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }
    }
}

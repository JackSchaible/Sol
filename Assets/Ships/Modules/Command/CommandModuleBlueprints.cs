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
            int health, int weight, int cost, ConnectorPositions[] connectors, int crewRequirement, int powerConumption,
            int commandSupplied)
            : base(ModuleTypes.ControlCentre, moduleType, buildSprite, name, description, health, weight, cost,
                connectors, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }
    }
}

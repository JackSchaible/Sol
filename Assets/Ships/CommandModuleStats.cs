namespace Assets.Ships
{
    public class CommandModuleStats : ModuleStats
    {
        public int CommandSupplied { get; set; }

        public CommandModuleStats(string moduleType, string buildSprite, string name, string description, int width, int height, 
            int offsetX, int offsetY, int originx, int originY, int health, int armor, int weight, int cost, int crewRequirement,
            int powerConumption, int commandSupplied)
            : base(moduleType, buildSprite, name, description, width, height, offsetX, offsetY, originx, originY, health, armor,
                  weight, cost, crewRequirement, powerConumption, 0)
        {
            CommandSupplied = commandSupplied;
        }
    }
}

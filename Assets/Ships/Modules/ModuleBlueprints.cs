using Assets.Ships.Modules;

namespace Assets.Ships
{
    public abstract class ModuleBlueprints
    {
        public ModuleTypes ModuleType { get; set; }
        public string ModuleSubtype { get; set; }
        public string BuildSprite { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Health { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public ConnectorPositions[] Connectors { get; set; }
        public ExclusionVectors[] ExclusionVectors { get; set; }
        public int CrewRequirement { get; set; }
        public int PowerConumption { get; set; }
        public int CommandRequirement { get; set; }
        public abstract string[] RelatedAbilities { get; }

        protected ModuleBlueprints()
        {

        }

        protected ModuleBlueprints(ModuleTypes moduleType, string moduleSubtype, string buildSprite, string name,
            string description, int health, int weight, int cost, ConnectorPositions[] connectors, 
            ExclusionVectors[] exclusionVectors, int crewRequirement, int powerConumption, int commandRequirement)
        {
            ModuleType = moduleType;
            ModuleSubtype = moduleSubtype;
            BuildSprite = buildSprite;
            Name = name;
            Description = description;
            Health = health;
            Weight = weight;
            Cost = cost;
            Connectors = connectors;
            ExclusionVectors = exclusionVectors;
            CrewRequirement = crewRequirement;
            PowerConumption = powerConumption;
            CommandRequirement = commandRequirement;
        }
    }
}

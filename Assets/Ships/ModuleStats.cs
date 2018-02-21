using System;

namespace Assets.Ships
{
    [Serializable]
    public abstract class ModuleStats
    {
        public string ModuleType { get; set; }
        public string BuildSprite { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Originx { get; set; }
        public int OriginY { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public int CrewRequirement { get; set; }
        public int PowerConumption { get; set; }
        public int CommandRequirement { get; set; }

        protected ModuleStats()
        {

        }

        protected ModuleStats(string moduleType, string buildSprite, string name, string description, int width, int height, 
            int offsetX, int offsetY, int originx, int originY, int health, int armor, int weight, int cost, int crewRequirement,
            int powerConumption, int commandRequirement)
        {
            ModuleType = moduleType;
            BuildSprite = buildSprite;
            Name = name;
            Description = description;
            Width = width;
            Height = height;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Originx = originx;
            OriginY = originY;
            Health = health;
            Armor = armor;
            Weight = weight;
            Cost = cost;
            CrewRequirement = crewRequirement;
            PowerConumption = powerConumption;
            CommandRequirement = commandRequirement;
        }
    }
}

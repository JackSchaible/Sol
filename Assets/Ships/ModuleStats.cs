using UnityEngine;

namespace Assets.Ships
{
    public abstract class ModuleStats
    {
        public ModuleTypes ModuleType { get; set; }
        public string ModuleSubtype { get; set; }
        public string BuildSprite { get; set; }
        public Vector3 Size { get; set; }
        public Vector2 Origin { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int Originx { get; set; }
        public int OriginY { get; set; }
        public int Health { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }
        public int CrewRequirement { get; set; }
        public int PowerConumption { get; set; }
        public int CommandRequirement { get; set; }
        public abstract string[] RelatedAbilities { get; }

        protected ModuleStats()
        {

        }

        protected ModuleStats(ModuleTypes moduleType, string moduleSubtype, string buildSprite, Vector3 size,
            Vector2 origin, string name, string description, int width, int height, int offsetX, int offsetY,
            int originx, int originY, int health, int weight, int cost, int crewRequirement, int powerConumption,
            int commandRequirement)
        {
            ModuleType = moduleType;
            ModuleSubtype = moduleSubtype;
            BuildSprite = buildSprite;
            Size = size;
            Origin = origin;
            Name = name;
            Description = description;
            Width = width;
            Height = height;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Originx = originx;
            OriginY = originY;
            Health = health;
            Weight = weight;
            Cost = cost;
            CrewRequirement = crewRequirement;
            PowerConumption = powerConumption;
            CommandRequirement = commandRequirement;
        }
    }
}

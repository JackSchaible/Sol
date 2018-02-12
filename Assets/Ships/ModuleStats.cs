using System;

namespace Assets.Ships
{
    [Serializable]
    public abstract class ModuleStats
    {
        public string ModuleName { get; set; }
        public string ModuleType { get; set; }
        public string BuildSprite { get; set; }
        public int Health { get; set; }
        public int Armor { get; set; }
        public int Weight { get; set; }
        public int Cost { get; set; }

        protected ModuleStats()
        {
            
        }

        protected ModuleStats(string moduleName, string moduleType, string buildSprite, int health, int armor,
            int weight, int cost)
        {
            ModuleName = moduleName;
            ModuleType = moduleType;
            BuildSprite = buildSprite;
            Health = health;
            Armor = armor;
            Weight = weight;
            Cost = cost;
        }
    }
}

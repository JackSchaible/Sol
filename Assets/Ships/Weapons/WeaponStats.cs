using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public class WeaponStats : ModuleStats
    {
        public int RateOfFire { get; set; }
        public WeaponDamage Damage { get; set; }
        public int Range { get; set; }
        public int PersonnelRequirement { get; set; }
        public int PowerConsumption { get; set; }
        public int AmmoStorage { get; set; }

        public WeaponStats()
        {
            
        }

        public WeaponStats(string moduleName, string moduleType, string buildSprite, int health, int armor, int weight,
            int cost, int rateOfFire, WeaponDamage damage, int range, int personnelRequirement, int powerConsumption,
            int ammoStorage)
            : base(moduleName, moduleType, buildSprite, health, armor, weight, cost)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            Range = range;
            PersonnelRequirement = personnelRequirement;
            PowerConsumption = powerConsumption;
            AmmoStorage = ammoStorage;
        }
    }
}

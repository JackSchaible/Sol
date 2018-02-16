using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public class WeaponStats : ModuleStats
    {
        public int RateOfFire { get; set; }
        public WeaponDamage Damage { get; set; }
        public int Range { get; set; }
        public int AmmoStorage { get; set; }

        public WeaponStats()
        {

        }

        public WeaponStats(string moduleType, string buildSprite, string name, int width, int height, int offsetX,
            int offsetY, int originX, int originY, int weight, int cost, int crewRequirement, int powerConumption,
            int commandRequirement, int rateOfFire, WeaponDamage damage, int range, int ammoStorage)
            : base(moduleType, buildSprite, name, width, height, offsetX, offsetY, originX, originY, 10, 0,
                 weight, cost, crewRequirement, powerConumption, commandRequirement)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            Range = range;
            AmmoStorage = ammoStorage;
        }
    }
}

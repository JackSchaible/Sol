using Assets.Ships.Modules;
using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public class WeaponBlueprint : ModuleBlueprints
    {
        public int RateOfFire { get; set; }
        public WeaponDamage Damage { get; set; }
        public int Range { get; set; }
        public int AmmoStorage { get; set; }
        public int DamageRadius { get; set; }
        public override string[] RelatedAbilities { get { return new [] { "Strength", "Dexterity"}; } }

        public WeaponBlueprint()
        {

        }

        public WeaponBlueprint(string moduleType, string buildSprite, string name, string description, int weight,
            int cost, ConnectorPosition[] connectors, ExclusionVectors[] exclusionVectors, int crewRequirement,
            int powerConumption, int commandRequirement, int rateOfFire, WeaponDamage damage, int range, int ammoStorage,
            int damageRadius)
            : base(ModuleTypes.Weapon, moduleType, buildSprite, name, description, 10, weight, cost,
                  connectors, exclusionVectors, crewRequirement, powerConumption, commandRequirement)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            Range = range;
            AmmoStorage = ammoStorage;
            DamageRadius = damageRadius;
        }
    }
}

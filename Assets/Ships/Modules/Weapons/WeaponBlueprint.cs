using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Weapons;

namespace Assets.Ships
{
    public class WeaponBlueprint : ModuleBlueprint
    {
        public int RateOfFire { get; set; }
        public int AmmoStorage { get; set; }
        public int DamageRadius { get; set; }
        public ProjectileEnergies ProjectileEnergies { get; set; }
        
        public override string[] RelatedAbilities { get { return new [] { "Strength", "Dexterity"}; } }

        public WeaponBlueprint()
        {

        }

        public WeaponBlueprint(string moduleType, string buildSprite, IntVector[] space, string name, 
            string description, float weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors, 
            ExclusionVector[] exclusionVectors, int crewRequirement, float powerConumption, int commandRequirement,
            int rateOfFire, ProjectileEnergies projectileEnergies, int ammoStorage, int damageRadius)
            : base(ModuleTypes.Weapon, moduleType, buildSprite, space, name, description, 10, weight, cost,
                  areConnectorsMandatory, connectors, exclusionVectors, crewRequirement, powerConumption, commandRequirement)
        {
            RateOfFire = rateOfFire;
            ProjectileEnergies = projectileEnergies;
            AmmoStorage = ammoStorage;
            DamageRadius = damageRadius;
        }
    }
}

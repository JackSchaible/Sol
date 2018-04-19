using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Weapons;
using UnityEngine;

namespace Assets.Ships
{
    public class WeaponBlueprint : ModuleBlueprint
    {
        public int RateOfFire { get; set; }
        public int Range { get; set; }
        public int AmmoStorage { get; set; }
        public int DamageRadius { get; set; }
        public ProjectileEnergies ProjectileEnergies { get; set; }
        
        public override string[] RelatedAbilities { get { return new [] { "Strength", "Dexterity"}; } }

        public WeaponBlueprint()
        {

        }

        public WeaponBlueprint(string moduleType, string buildSprite, List<IntVector> space, string name, 
            string description, int weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors, 
            ExclusionVector[] exclusionVectors, int crewRequirement, int powerConumption, int commandRequirement,
            int rateOfFire, ProjectileEnergies projectileEnergies, int range, int ammoStorage, int damageRadius)
            : base(ModuleTypes.Weapon, moduleType, buildSprite, space, name, description, 10, weight, cost,
                  areConnectorsMandatory, connectors, exclusionVectors, crewRequirement, powerConumption, commandRequirement)
        {
            RateOfFire = rateOfFire;
            ProjectileEnergies = projectileEnergies;
            Range = range;
            AmmoStorage = ammoStorage;
            DamageRadius = damageRadius;
        }
    }
}

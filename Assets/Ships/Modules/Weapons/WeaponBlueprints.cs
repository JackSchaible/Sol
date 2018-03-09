using System;
using Assets.Ships.Modules;
using Assets.Ships.Weapons;
using UnityEngine;

namespace Assets.Ships
{
    public class WeaponBlueprints : ModuleBlueprints
    {
        public int RateOfFire { get; set; }
        public WeaponDamage Damage { get; set; }
        public int Range { get; set; }
        public int AmmoStorage { get; set; }
        public override string[] RelatedAbilities { get { return new [] { "Strength", "Dexterity"}; } }

        public WeaponBlueprints()
        {

        }

        public WeaponBlueprints(string moduleType, string buildSprite, string name, string description, int weight,
            int cost, ConnectorPositions[] connectors, ExclusionVectors[] exclusionVectors, int crewRequirement,
            int powerConumption, int commandRequirement, int rateOfFire, WeaponDamage damage, int range, int ammoStorage)
            : base(ModuleTypes.Weapon, moduleType, buildSprite, name, description, 10, weight, cost,
                  connectors, exclusionVectors, crewRequirement, powerConumption, commandRequirement)
        {
            RateOfFire = rateOfFire;
            Damage = damage;
            Range = range;
            AmmoStorage = ammoStorage;
        }
    }
}

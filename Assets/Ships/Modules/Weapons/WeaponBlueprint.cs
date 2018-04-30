using Assets.Common.Utils;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Weapons;
using UnityEngine;

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

        public WeaponBlueprint(string moduleType, string buildSprite, Vector3Int[] space, string name, 
            string description, float weight, Cost cost, bool areConnectorsMandatory, Connector[] connectors, 
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

        public override ModuleBlueprint Copy()
        {
            var connectors = new Connector[Connectors.Length];
            var exclusionVectors = new ExclusionVector[ExclusionVectors.Length];
            var space = new Vector3Int[Space.Length];

            Connectors.CopyTo(connectors, 0);
            ExclusionVectors.CopyTo(exclusionVectors, 0);
            Space.CopyTo(space, 0);

            return new WeaponBlueprint(ModuleSubtype, BuildSprite, space, Name, Description, Weight,
                Cost, AreConnectorsMandatory, connectors, exclusionVectors, CrewRequirement, PowerConumption,
                CommandRequirement, RateOfFire, ProjectileEnergies, AmmoStorage, DamageRadius);
        }
    }
}

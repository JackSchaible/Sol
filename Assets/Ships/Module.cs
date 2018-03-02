using System.Collections.Generic;
using Assets.Ships.Crew;
using Assets.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Ships
{
    public class Module
    {
        public ModuleStats ModuleStats { get; set; }

        public GameObject GameObject;
        public int Health;
        public int CurrentPower;
        public List<CrewMember> Crew;
        public Vector3 Position;
        public int OwnerId;
        public double Efficiency;

        private Module(ModuleStats moduleStats)
        {
            ModuleStats = moduleStats;
            Crew = new List<CrewMember>();
        }

        public static Module Create(ModuleStats stats)
        {
            var module = new Module(stats);
            module.GameObject = new GameObject();
            module.GameObject.AddComponent<Image>();
            module.GameObject.GetComponent<Image>().sprite = GraphicsUtils.GetSpriteFromPath(stats.BuildSprite);

            return module;
        }

        public void CalculateEfficiency()
        {
            var eff = CalculateCrewEfficiency();
            eff *= ((float)CurrentPower / (float)ModuleStats.PowerConumption);
            //TODO: any other mods?

            Efficiency = eff;
        }

        private float CalculateCrewEfficiency()
        {
            float totalMod = 0;

            foreach (var crew in Crew)
                foreach (var ability in ModuleStats.RelatedAbilities)
                {
                    float abilityMod = 0f;

                    switch (ability)
                    {
                        case "Strength":
                            abilityMod = crew.Strength;
                            break;

                        case "Dexterity":
                            abilityMod = crew.Dexterity;
                            break;

                        case "Constitution":
                            abilityMod = crew.Constitution;
                            break;

                        case "Intelligence":
                            abilityMod = crew.Intelligence;
                            break;

                        case "Wisdom":
                            abilityMod = crew.Wisdom;
                            break;

                        case "Charisma":
                            abilityMod = crew.Charisma;
                            break;
                    }

                    abilityMod /= (ModuleStats.RelatedAbilities.Length * 10);
                    abilityMod += 1;
                    totalMod += (int)Mathf.Floor((abilityMod - 10.0f) / 2f);
                }

            return totalMod;
        }
    }
}

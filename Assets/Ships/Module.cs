using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Crew;
using Assets.Utils;
using UnityEngine;

namespace Assets.Ships
{
    public class Module
    {
        public ModuleBlueprint ModuleBlueprint { get; set; }

        public GameObject GameObject;
        public int CurrentHealth;
        public int CurrentPower;
        public List<CrewMember> Crew;
        public IntVector Position;
        public int OwnerId;
        public double Efficiency;

        public Module()
        {
            
        }

        private Module(ModuleBlueprint moduleBlueprint)
        {
            ModuleBlueprint = moduleBlueprint;
            Crew = new List<CrewMember>();
        }

        public static Module Create(ModuleBlueprint blueprint)
        {
            var module = new Module(blueprint);
            module.GameObject = new GameObject();
            module.GameObject.AddComponent<SpriteRenderer>();
            module.GameObject.GetComponent<SpriteRenderer>().sprite = GraphicsUtils.GetSpriteFromPath(blueprint.BuildSprite);
            module.GameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI FG";
            module.GameObject.AddComponent<BoxCollider>();
            

            return module;
        }

        public void Initialize()
        {
            GameObject.AddComponent<SpriteRenderer>();
            GameObject.GetComponent<SpriteRenderer>().sprite = GraphicsUtils.GetSpriteFromPath(ModuleBlueprint.BuildSprite);
            GameObject.GetComponent<SpriteRenderer>().sortingLayerName = "UI BG";
        }

        public void CalculateEfficiency()
        {
            var eff = CalculateCrewEfficiency();
            eff *= ((float)CurrentPower / (float)ModuleBlueprint.PowerConumption);
            //TODO: any other mods?

            Efficiency = eff;
        }

        private float CalculateCrewEfficiency()
        {
            float totalMod = 0;

            foreach (var crew in Crew)
                foreach (var ability in ModuleBlueprint.RelatedAbilities)
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

                    abilityMod /= (ModuleBlueprint.RelatedAbilities.Length * 10);
                    abilityMod += 1;
                    totalMod += (int)Mathf.Floor((abilityMod - 10.0f) / 2f);
                }

            return totalMod;
        }
    }
}

using System.Collections.Generic;
using Assets.Ships.Crew;
using Assets.Ships.Modules;
using Assets.Utils;
using UnityEngine;

namespace Assets.Ships
{
    public class Module
    {
        public ModuleBlueprint ModuleBlueprint { get; set; }

        public int CurrentHealth;
        public int CurrentPower;
        public List<CrewMember> Crew;
        public Vector3Int Position;
        public int OwnerId;
        public double Efficiency;
        public ModuleComponent[] Components { get; set; }

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
            var module = new Module(blueprint.Copy());
            var components = new List<ModuleComponent>();

            foreach (var space in module.ModuleBlueprint.Space)
            {
                List<Connector> connectors = new List<Connector>();
                List<ExclusionVector> exclusionVectors = new List<ExclusionVector>();
                    
                foreach (Connector connector in module.ModuleBlueprint.Connectors)
                    if (connector.Position == space)
                        connectors.Add(connector);

                foreach (ExclusionVector vector in module.ModuleBlueprint.ExclusionVectors)
                    if (vector.Position == space)
                        exclusionVectors.Add(vector);

                var go = new GameObject();
                var sr = go.AddComponent<SpriteRenderer>();
                var buildSprite = GraphicsUtils.GetSpriteFromPath(blueprint.ComponentBuildSprites[space.x, space.y, space.z], true);
                var sprite = GraphicsUtils.GetSpriteFromPath(blueprint.ComponentSprites[space.x, space.y, space.z], true);
                sr.sprite = buildSprite;
                sr.sortingLayerName = "UI FG";

                components.Add(new ModuleComponent(go, buildSprite, sprite, space, connectors.ToArray(), exclusionVectors.ToArray()));
            }

            module.Components = components.ToArray();

            return module;
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

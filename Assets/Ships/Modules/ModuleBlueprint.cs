using Assets.Ships.Modules;
using UnityEngine;

namespace Assets.Ships
{
    public abstract class ModuleBlueprint
    {
        public ModuleTypes ModuleType { get; set; }
        public string ModuleSubtype { get; set; }
        public string BuildSprite { get; set; }
        public string[,,] ComponentSprites { get; set; }
        public Vector3Int[] Space { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Health { get; set; }
        public float Weight { get; set; }
        public Cost Cost { get; set; }
        public Connector[] Connectors { get; set; }
        public bool AreConnectorsMandatory { get; set; }
        public ExclusionVector[] ExclusionVectors { get; set; }
        public int CrewRequirement { get; set; }
        public float PowerConumption { get; set; }
        public int CommandRequirement { get; set; }
        public abstract string[] RelatedAbilities { get; }

        protected ModuleBlueprint()
        {

        }

        protected ModuleBlueprint(ModuleTypes moduleType, string moduleSubtype, string buildSprite,
            string[,,] componentSprites, Vector3Int[] space, string name, string description,
            int health, float weight, Cost cost, Connector[] connectors, bool areConnectorsMandatory,
            ExclusionVector[] exclusionVectors, int crewRequirement, float powerConumption,
            int commandRequirement)
        {
            ModuleType = moduleType;
            ModuleSubtype = moduleSubtype;
            BuildSprite = buildSprite;
            ComponentSprites = componentSprites;
            Space = space;
            Name = name;
            Description = description;
            Health = health;
            Weight = weight;
            Cost = cost;
            Connectors = connectors;
            AreConnectorsMandatory = areConnectorsMandatory;
            ExclusionVectors = exclusionVectors;
            CrewRequirement = crewRequirement;
            PowerConumption = powerConumption;
            CommandRequirement = commandRequirement;
        }

        public abstract ModuleBlueprint Copy();
    }
}

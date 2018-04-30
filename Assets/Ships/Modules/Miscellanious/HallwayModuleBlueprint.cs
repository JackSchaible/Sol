using Assets.Common.Utils;
using UnityEngine;

namespace Assets.Ships.Modules.Miscellanious
{
    public class HallwayModuleBlueprint : ModuleBlueprint
    {
        public override string[] RelatedAbilities
        {
            get { return new string[0]; }
        }

        public HallwayModuleBlueprint(string buildSprite, Vector3Int[] space, string name, string description,
            int health, float weight, Cost cost, bool areConnectorsMandatory, Connector[] connectors, ExclusionVector[] exclusionVectors)
            : base(ModuleTypes.Miscellanious, MiscellaniousTypes.Decorative, buildSprite, space, name, description,
                health, weight, cost, areConnectorsMandatory, connectors, exclusionVectors, 0, 0, 0)
        {
            
        }

        public override ModuleBlueprint Copy()
        {
            var connectors = new Connector[Connectors.Length];
            var exclusionVectors = new ExclusionVector[ExclusionVectors.Length];
            var space = new Vector3Int[Space.Length];

            Connectors.CopyTo(connectors, 0);
            ExclusionVectors.CopyTo(exclusionVectors, 0);
            Space.CopyTo(space, 0);

            return new HallwayModuleBlueprint(BuildSprite, space, Name, Description, Health, Weight, Cost,
                AreConnectorsMandatory, connectors, exclusionVectors);
        }
    }
}

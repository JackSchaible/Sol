using System.Collections.Generic;
using Assets.Common.Utils;

namespace Assets.Ships.Modules.Miscellanious
{
    public class HallwayModuleBlueprint : ModuleBlueprint
    {
        public override string[] RelatedAbilities
        {
            get { return new string[0]; }
        }

        public HallwayModuleBlueprint(string buildSprite, IntVector[] space, string name, string description,
            int health, float weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors, ExclusionVector[] exclusionVectors)
            : base(ModuleTypes.Miscellanious, MiscellaniousTypes.Decorative, buildSprite, space, name, description,
                health, weight, cost, areConnectorsMandatory, connectors, exclusionVectors, 0, 0, 0)
        {
            
        }
    }
}

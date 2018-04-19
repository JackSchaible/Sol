using System.Collections.Generic;
using Assets.Common.Utils;

namespace Assets.Ships.Modules.Miscellanious
{
    public class DecorativeModuleBlueprint : ModuleBlueprint
    {
        public override string[] RelatedAbilities
        {
            get { return new string[0]; }
        }

        public DecorativeModuleBlueprint(string buildSprite, List<IntVector> space, string name, string description,
            int health, int weight, Cost cost, bool areConnectorsMandatory, ConnectorPosition[] connectors, ExclusionVector[] exclusionVectors)
            : base(ModuleTypes.Miscellanious, MiscellaniousTypes.Decorative, buildSprite, space, name, description,
                health, weight, cost, areConnectorsMandatory, connectors, exclusionVectors, 0, 0, 0)
        {

        }
    }
}

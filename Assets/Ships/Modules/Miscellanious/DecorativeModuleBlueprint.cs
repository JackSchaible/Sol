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
            int health, int weight, int cost, ConnectorPosition[] connectors, ExclusionVectors[] exclusionVectors)
            :base(ModuleTypes.Miscellanious, MiscellaniousTypes.Decorative, buildSprite, space, name, description,
                 health, weight, cost, connectors, exclusionVectors, 0, 0, 0)
        {
            
        }
    }
}

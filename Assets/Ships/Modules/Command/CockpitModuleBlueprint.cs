using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;

namespace Assets.Ships
{
    public class CockpitModuleBlueprint : CommandModuleBlueprint
    {
        public int PersonnelHoused { get; set; }

        public CockpitModuleBlueprint()
        {
            
        }

        public CockpitModuleBlueprint(string moduleType, string buildSprite, List<IntVector> space, string name,
            string description, int health, int weight, int cost, ConnectorPosition[] connectors, int crewRequirement,
            int powerConumption, int personnelHoused, int commandSupplied)
            : base(moduleType, buildSprite, space, name, description, health, weight, cost, connectors,
                  new []{ Modules.ExclusionVectors.PlaneAndForward }, crewRequirement, powerConumption, commandSupplied)
        {
            PersonnelHoused = personnelHoused;
        }
    }
}

using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Miscellanious;
using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public class ModuleBlueprintsManager
    {
        public ModuleBlueprintsManager()
        {
        }

        public ModuleBlueprint[] Get()
        {
            return Generate();
        }

        private ModuleBlueprint[] Generate()
        {
            return new ModuleBlueprint[]
            {
                #region Command Modules

                #region Cockpit Modules

                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Basic Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Basic Cockpit",
                    "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                    25, 500, 25, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 1, 200, 1, 8),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Advanced Cockpit",
                    "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.",
                    25, 550, 35, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 1, 500, 1,
                    12),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Heavy Cockpit",
                    "A basic cockpit with a bit more space for 2 pilots.",
                    40, 750, 30, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 250, 2,
                    16),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    new List<IntVector> {IntVector.Zero}, "Advanced Heavy Cockpit",
                    "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 800, 50, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 550, 2,
                    24),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right},
                    "Tactical Cockpit",
                    "A much larger cockpit, usually reserved for long-range strike craft, scout craft, bombers, or anything else that requires someone other than a pilot. Has space for 2 pilots and 1 technical officer. Includes a more advanced AI for helping with technical tasks, as well as maintaining life support and doing navigation calculations, among other things.",
                    75, 1500, 100,
                    new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward, Position = IntVector.Right}},
                    3, 1000, 3, 36),

                #endregion

                #endregion

                #region Weapons

                #region Projectiles

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/LMG - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Light Machine Gun",
                    "A lighter machine gun firing smaller rounds at a higher rate of fire. Does poorly against shields and armor, but the higher projectile speed means higher accuracy at longer ranges.",
                    200, 15, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {ExclusionVectors.RightLine}, 0, 200, 1, 6000, new WeaponDamage(1, 1, 0, 0), 700, 15000, 1),

                #endregion

                #endregion

                #region Miscellanious

                #region Decorative Modules

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel - Build",
                    new List<IntVector> {IntVector.Zero}, "Panel",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 0, 0, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward),
                        new ConnectorPosition(ConnectorPositions.Left),
                        new ConnectorPosition(ConnectorPositions.Down),
                        new ConnectorPosition(ConnectorPositions.Forward),
                        new ConnectorPosition(ConnectorPositions.Right),
                        new ConnectorPosition(ConnectorPositions.Up)
                    },
                    new ExclusionVectors[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Crossbeam Connector - Build",
                    new List<IntVector> {IntVector.Zero},
                    "Crossbeam Connector",
                    "A basic connector. Provides no armor or hull. Holds pieces of your ship together on the top or bottom.",
                    0, 0, 0, new ConnectorPosition[]
                    {
                        new ConnectorPosition(ConnectorPositions.Down),
                        new ConnectorPosition(ConnectorPositions.Up)
                    },
                    new ExclusionVectors[] { }),

                #endregion

                #endregion
            };
        }
    }
}

using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Miscellanious;
using Assets.Ships.Weapons;

namespace Assets.Ships
{
    public static class ModuleBlueprintsManager
    {

        public static ModuleBlueprint[] Generate()
        {
            var blueprints = new List<ModuleBlueprint>();

            blueprints.AddRange(GetCommandModules());
            blueprints.AddRange(GetWeapons());
            blueprints.AddRange(GetMiscellaniousModules());

            return blueprints.ToArray();
        }

        #region Command Modules

        private static CommandModuleBlueprint[] GetCommandModules()
        {
            var commandModules = new List<CommandModuleBlueprint>();

            commandModules.AddRange(GetCockpits());

            return commandModules.ToArray();
        }

        private static CockpitModuleBlueprint[] GetCockpits()
        {
            return new[]
            {
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Basic Cockpit - Build", new List<IntVector>
                    {
                        IntVector.Zero
                    },
                    "Basic Cockpit",
                    "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                    25, 500, 25, false, new[]
                    {
                        new ConnectorPosition
                        {
                            Direction = ConnectorPositions.Backward
                        }
                    }, 1, 200, 1, 8),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Advanced Cockpit",
                    "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.",
                    25, 550, 35, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 1, 500,
                    1,
                    12),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Heavy Cockpit",
                    "A basic cockpit with a bit more space for 2 pilots.",
                    40, 750, 30, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 250,
                    2,
                    16),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    new List<IntVector> {IntVector.Zero}, "Advanced Heavy Cockpit",
                    "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 800, 50, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 550,
                    2,
                    24),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right},
                    "Tactical Cockpit",
                    "A much larger cockpit, usually reserved for long-range strike craft, scout craft, bombers, or anything else that requires someone other than a pilot. Has space for 2 pilots and 1 technical officer. Includes a more advanced AI for helping with technical tasks, as well as maintaining life support and doing navigation calculations, among other things.",
                    75, 1500, 100,
                    false,
                    new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward, Position = IntVector.Right}},
                    3, 1000, 3, 36)
            };
        }

        #endregion

        #region Weapons

        private static WeaponBlueprint[] GetWeapons()
        {
            var weapons = new List<WeaponBlueprint>();

            weapons.AddRange(GetProjectiles());

            return weapons.ToArray();
        }

        private static WeaponBlueprint[] GetProjectiles()
        {
            return new[]
            {
                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/LMG - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Light Machine Gun",
                    "A lighter machine gun firing smaller rounds at a higher rate of fire. Does poorly against shields and armor, but the higher projectile speed means higher accuracy at longer ranges.",
                    200, 15, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 0, 200, 1, 6000,
                    new WeaponDamage(1, 1, 0, 0), 700, 15000, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HMG - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Heavy Machine Gun",
                    "A heavier machine gun boasting higher damage at the cost of power consumption, ammo storage, rate of fire, and accuracy.",
                    400, 20, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 0, 400, 1, 3000,
                    new WeaponDamage(3, 3, 2, 1), 1000, 6800, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/AA Gun - Build",
                    new List<IntVector>
                    {
                        IntVector.Zero,
                        IntVector.Up,
                        new IntVector(1, 1, 0),
                        new IntVector(2, 1, 0),
                        new IntVector(3, 1, 0),
                        IntVector.Right,
                        new IntVector(2, 0, 0),
                        new IntVector(3, 1, 0)
                    }, "Flak Cannon",
                    "A four-barreled, high rate-of-fire cannon that fires explosive shells filled with shrapnel. These projectiles only detonate when they come in proximity of enemy strike craft. Useless against larger ships.",
                    750, 30, true, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Zero),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Right),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, false, new IntVector(1, 1, 0))
                    },
                    new[]
                    {
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, IntVector.Up)
                    }, 4, 1000, 2, 5000, new WeaponDamage(20, 20, 0, 0), 4000, 11250, 5),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HE Cannon - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "HE Cannon",
                    "A gun firing high-explosive (HE) projectiles. These guns fire bullets filled with acids or powders that ignite on impact, creating large explosions on impact.",
                    1000, 50, false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 6, 2000, 4, 2000,
                    new WeaponDamage(10, 10, 5, 5), 5000, 4500, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Amber Gun - Build",
                    new List<IntVector>
                    {
                        IntVector.Zero,
                        IntVector.Up,
                        new IntVector(1, 1, 0),
                        new IntVector(2, 1, 0),
                        new IntVector(3, 1, 0),
                        IntVector.Right,
                        new IntVector(2, 0, 0),
                        new IntVector(3, 1, 0)
                    }, "Amber Quarantine Gun",
                    "A gun that fires a projectile which erupts with an inert gas, covering but not harming a 10-m radius with a thick smog. After 5s, the smog solidifies into a substance harder than steel.",
                    1750, 85, true, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Zero),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Right),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, false, new IntVector(1, 1, 0))
                    },
                    new[]
                    {
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, IntVector.Up)
                    }, 8, 4000, 6, 1500, new WeaponDamage(0, 0, 0, 0), 2000, 3500, 10)
            };
        }

        #endregion

        #region Miscellanious

        private static ModuleBlueprint[] GetMiscellaniousModules()
        {
            var miscellaniousModules = new List<ModuleBlueprint>();

            miscellaniousModules.AddRange(GetDecorativeModules());
            miscellaniousModules.AddRange(GetHallwayModules());

            return miscellaniousModules.ToArray();
        }

        private static DecorativeModuleBlueprint[] GetDecorativeModules()
        {
            return new []
            {
                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel - Build",
                    new List<IntVector> {IntVector.Zero}, "Panel",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                        new ConnectorPosition(ConnectorPositions.Left, false),
                        new ConnectorPosition(ConnectorPositions.Down, false),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Right, false),
                        new ConnectorPosition(ConnectorPositions.Up, false)
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel 2 - Build",
                    new List<IntVector> {IntVector.Zero}, "Panel 2",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                        new ConnectorPosition(ConnectorPositions.Left, false),
                        new ConnectorPosition(ConnectorPositions.Down, false),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Right, false),
                        new ConnectorPosition(ConnectorPositions.Up, false)
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Crossbeam Connector - Build",
                    new List<IntVector> {IntVector.Zero},
                    "Crossbeam Connector",
                    "A basic connector. Provides no armor or hull. Holds pieces of your ship together on the top or bottom.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Down, false),
                        new ConnectorPosition(ConnectorPositions.Up, false)
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Weapon Mount - Build",
                    new List<IntVector> {IntVector.Zero},
                    "Weapon Mount",
                    "A simple weapon mount that allows small weapons to be attached to the underside of panels or wings.",
                    0, 250, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Up, false)
                    },
                    new ExclusionVector[] { })
            };
        }
        private static HallwayModuleBlueprint[] GetHallwayModules()
        {
            return new[]
            {

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/EW Hallway - Build",
                    new List<IntVector> {IntVector.Zero},
                    "EW Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left, true),
                        new ConnectorPosition(ConnectorPositions.Right, true),
                        new ConnectorPosition(ConnectorPositions.Up, false),
                        new ConnectorPosition(ConnectorPositions.Down, false),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NS Hallway - Build",
                    new List<IntVector> {IntVector.Zero},
                    "NS Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left, false),
                        new ConnectorPosition(ConnectorPositions.Right, false),
                        new ConnectorPosition(ConnectorPositions.Up, true),
                        new ConnectorPosition(ConnectorPositions.Down, true),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NE-SW Hallway - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Up},
                    "NE-SW Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left, true),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                        new ConnectorPosition(ConnectorPositions.Right, true, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Forward, false, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Up),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NW-SE Hallway - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Up},
                    "NW-SE Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, 0, false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Right, true),
                        new ConnectorPosition(ConnectorPositions.Forward, false),
                        new ConnectorPosition(ConnectorPositions.Backward, false),
                        new ConnectorPosition(ConnectorPositions.Left, true, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Forward, false, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, false, IntVector.Up),
                    },
                    new ExclusionVector[] { }),
            };
        }

        #endregion
    }
}

using System.Collections.Generic;
using Assets.Common.Utils;
using Assets.Data;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Miscellanious;
using Assets.Ships.Modules.Weapons;

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
                    25, 500, new Cost(1, 1, 1, 2, 0), false, new[]
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
                    25, 550, new Cost(1, 3, 1, 2, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 1, 500,
                    1,
                    12),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build", new List<IntVector> {IntVector.Zero},
                    "Heavy Cockpit",
                    "A basic cockpit with a bit more space for 2 pilots.",
                    40, 750, new Cost(1, 2, 1, 1, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 250,
                    2,
                    16),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    new List<IntVector> {IntVector.Zero}, "Advanced Heavy Cockpit",
                    "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 800, new Cost(1, 4, 1, 3, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Backward}}, 2, 550,
                    2,
                    24),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right},
                    "Tactical Cockpit",
                    "A much larger cockpit, usually reserved for long-range strike craft, scout craft, bombers, or anything else that requires someone other than a pilot. Has space for 2 pilots and 1 technical officer. Includes a more advanced AI for helping with technical tasks, as well as maintaining life support and doing navigation calculations, among other things.",
                    75, 1500, new Cost(2, 5, 2, 5, 0),
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
                    200, new Cost(0, 0, 0, 10, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 0, 200, 1, 6000,
                    new ProjectileEnergies(2050, 463, 0), 15000, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HMG - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Heavy Machine Gun",
                    "A heavier machine gun boasting higher damage at the cost of power consumption, ammo storage, rate of fire, and accuracy.",
                    400, new Cost(0, 0, 0, 58, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 0, 400, 1, 3000,
                    new ProjectileEnergies(17088, 924, 0), 6800, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Chain Gun - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Chain Gun",
                    "This weapon has six barrels that rotate rapidly. It uses energy from the ship's power source to reload the weapon chambers, rather than diverting energy from the combustion of the projectile. The result is an extremely high rate of fire with negligible energy drain from the projectiles.",
                    750, new Cost(0, 0, 0, 112, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Forward}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 0, 750, 2, 12000,
                    new ProjectileEnergies(56448, 11576, 0), 27000, 1),

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
                    750, new Cost(0, 0, 2750, 4750, 0), true, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Zero),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Right),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, new IntVector(1, 1, 0))
                    },
                    new[]
                    {
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, IntVector.Up)
                    }, 4, 1000, 2, 5000, new ProjectileEnergies(1920643, 752166, 18328160), 11250, 5),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HE Cannon - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "HE Cannon",
                    "A gun firing high-explosive (HE) projectiles. These guns fire bullets filled with acids or powders that ignite on impact, creating large explosions on impact.",
                    1000, new Cost(0, 0, 1243, 6757, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 6, 2000, 4, 2000,
                    new ProjectileEnergies(20270460, 2172045, 14504000), 4500, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Incendiary Cannon - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Right}, "Incendiary Cannon",
                    "A gun firing a shell filled with a combustible phosphorus powder. The powder is spread on contact, and subsequently ignites, burning anyone or anything caught in the cloud. Does high damage against flesh and hull, a small bit of damage to shields, but no damage to armor.",
                    1000, new Cost(0, 0, 3000, 7000, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Down}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 8, 2500, 6, 2500,
                    new ProjectileEnergies(19868750, 2210630, 30000000), 4500, 1),

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
                    1750, new Cost(2000, 0, 3000, 72500, 0), true, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Zero),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Right),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat}, new IntVector(1, 1, 0))
                    },
                    new[]
                    {
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, IntVector.Up)
                    }, 8, 4000, 6, 1500, new ProjectileEnergies(12500000, 803865, 0), 3500, 10),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Tracer Gun - Build",
                    new List<IntVector> {IntVector.Zero}, "Tracer Gun",
                    "A gun that fires a projectile that emits a moderately-strong radio frequency. On a hit, it embeds itself in the hull or skin of an enemy, allowing your other weapons to more accurately target that unit. Negates the cloaking units of any enemies hit.",
                    1000, new Cost(0, 0.03f, 0, 320, 0), false, new[] {new ConnectorPosition {Direction = ConnectorPositions.Forward}},
                    new[] {new ExclusionVector(new[] {ExclusionVectorDirections.RightLine})}, 3, 5000, 2, 500,
                    new ProjectileEnergies(625000, 500, 0), 1000, 0),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon - Build",
                    new List<IntVector>
                    {
                        new IntVector(0, 0, 0), new IntVector(0, 1, 0), new IntVector(0, 2, 0), new IntVector(0, 3, 0), new IntVector(0, 4, 0), new IntVector(0, 5, 0),
                        new IntVector(1, 0, 0), new IntVector(1, 1, 0), new IntVector(1, 2, 0), new IntVector(1, 3, 0), new IntVector(1, 4, 0), new IntVector(1, 5, 0),
                        new IntVector(2, 2, 0), new IntVector(2, 3, 0), new IntVector(2, 4, 0), new IntVector(2, 5, 0),
                        new IntVector(3, 2, 0), new IntVector(3, 3, 0),
                        new IntVector(4, 2, 0), new IntVector(4, 3, 0),
                        new IntVector(5, 2, 0), new IntVector(5, 3, 0),

                        new IntVector(0, 0, 1), new IntVector(0, 1, 1), new IntVector(0, 2, 1), new IntVector(0, 3, 1), new IntVector(0, 4, 1), new IntVector(0, 5, 1),
                        new IntVector(1, 0, 1), new IntVector(1, 1, 1), new IntVector(1, 2, 1), new IntVector(1, 3, 1), new IntVector(1, 4, 1), new IntVector(1, 5, 1),
                        new IntVector(2, 2, 1), new IntVector(2, 3, 1), new IntVector(2, 4, 1), new IntVector(2, 5, 1),
                        new IntVector(3, 2, 1), new IntVector(3, 3, 1),
                        new IntVector(4, 2, 1), new IntVector(4, 3, 1),
                        new IntVector(5, 2, 1), new IntVector(5, 3, 1),
                    }, "Depleted-Uranium Slug Cannon",
                    "A large cannon that fires hyper-dense slugs of an alloy composed of tungsten and uranium-238 (aka, depleted uranium). Requires a large explosive force to fire the projectile. Less accurate than the other weapons, but does high damage against hull/flesh and armor.",
                    50000000, new Cost(0, 0, 0, 2000000, 36720), true, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 0, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 1, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 2, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 3, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 4, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 5, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(1, 0, 0)),
                        new ConnectorPosition(ConnectorPositions.Backward, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(1, 1, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 0, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 1, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 2, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 3, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 4, 0)),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air}, new IntVector(0, 5, 0)),
                    },
                    new[]
                    {
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, new IntVector(0, 2, 0)),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, new IntVector(0, 3, 0)),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, new IntVector(0, 2, 1)),
                        new ExclusionVector(new[] {ExclusionVectorDirections.RightLine}, new IntVector(0, 3, 1))
                    }, 10, 10000, 5, 250, new ProjectileEnergies(14994E6f, 3587250240, 0), 750, 3),
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
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Left, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Down, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Right, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Up, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel 2 - Build",
                    new List<IntVector> {IntVector.Zero}, "Panel 2",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Left, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Down, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Right, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Up, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Crossbeam Connector - Build",
                    new List<IntVector> {IntVector.Zero},
                    "Crossbeam Connector",
                    "A basic connector. Provides no armor or hull. Holds pieces of your ship together on the top or bottom.",
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Down, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Up, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Weapon Mount - Build",
                    new List<IntVector> {IntVector.Zero},
                    "Weapon Mount",
                    "A simple weapon mount that allows small weapons to be attached to the underside of panels or wings.",
                    0, 250, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Forward, new []{Materials.Power, Materials.Heat}),
                        new ConnectorPosition(ConnectorPositions.Up, new []{Materials.Power, Materials.Heat})
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
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Right, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Up, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Down, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NS Hallway - Build",
                    new List<IntVector> {IntVector.Zero},
                    "NS Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Right, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Up,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Down,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NE-SW Hallway - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Up},
                    "NE-SW Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Left,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Right,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0], IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0], IntVector.Up),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NW-SE Hallway - Build",
                    new List<IntVector> {IntVector.Zero, IntVector.Up},
                    "NW-SE Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new[]
                    {
                        new ConnectorPosition(ConnectorPositions.Right, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0]),
                        new ConnectorPosition(ConnectorPositions.Left, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}, IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Forward, new Materials[0], IntVector.Up),
                        new ConnectorPosition(ConnectorPositions.Backward, new Materials[0], IntVector.Up),
                    },
                    new ExclusionVector[] { }),
            };
        }

        #endregion
    }
}

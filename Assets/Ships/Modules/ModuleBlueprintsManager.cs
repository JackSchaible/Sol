using System.Collections.Generic;
using Assets.Data;
using Assets.Ships.Modules.Command;
using Assets.Ships.Modules.Miscellanious;
using Assets.Ships.Modules.Weapons;
using UnityEngine;

namespace Assets.Ships.Modules
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
            return new []
            {
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Basic Cockpit - Build",
                    new [,,]{{{"Ships/Control Centres/Small Ship/Basic Cockpit - Build"}}},
                    new [] { Vector3Int.zero }, "Basic Cockpit",
                    "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                    25, 500, new Cost(1, 1, 1, 2, 0), false, new []
                    {
                        new Connector
                        {
                            Direction = new Vector3Int(0, 0, -1)
                        }
                    }, 1, 200, 1, 8),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Cockpit - Build",
                    new [,,]{{{"Ships/Control Centres/Small Ship/Advanced Cockpit - Build"}}},
                    new [] {Vector3Int.zero}, "Advanced Cockpit", 
                    "A slightly more advanced cockpit. Contains a very basic VI for assistance in navigation, communications, life support management, and other menial piloting tasks.",
                    25, 550, new Cost(1, 3, 1, 2, 0), false, new [] {new Connector {Direction = new Vector3Int(0, 0, -1) } }, 1, 500,
                    1,
                    12),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Heavy Cockpit - Build",
                    new [,,]{{{"Ships/Control Centres/Small Ship/Heavy Cockpit - Build"}}},
                    new [] {Vector3Int.zero}, "Heavy Cockpit",
                    "A basic cockpit with a bit more space for 2 pilots.",
                    40, 750, new Cost(1, 2, 1, 1, 0), false, new [] {new Connector {Direction = new Vector3Int(0, 0, -1) } }, 2, 250,
                    2,
                    16),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    new [,,]{{{"Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build"}}},
                    new [] {Vector3Int.zero}, "Advanced Heavy Cockpit",
                    "A larger cockpit boasting 2 crew and 2 VI's.",
                    50, 800, new Cost(1, 4, 1, 3, 0), false, new [] {new Connector {Direction = new Vector3Int(0, 0, -1) } }, 2, 550,
                    2,
                    24),
                new CockpitModuleBlueprint(ControlCentreTypes.SmallShip,
                    "Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    new [,,]
                    {
                        {{"Ships/Control Centres/Small Ship/Tactical Cockpit (0, 0, 0)"}},
                        {{"Ships/Control Centres/Small Ship/Tactical Cockpit (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "Tactical Cockpit",
                    "A much larger cockpit, usually reserved for long-range strike craft, scout craft, bombers, or anything else that requires someone other than a pilot. Has space for 2 pilots and 1 technical officer. Includes a more advanced AI for helping with technical tasks, as well as maintaining life support and doing navigation calculations, among other things.",
                    75, 1500, new Cost(2, 5, 2, 5, 0),
                    false,
                    new []
                    {
                        new Connector {Direction = new Vector3Int(0, 0, -1), Position = Vector3Int.zero},
                        new Connector {Direction = new Vector3Int(0, 0, -1), Position = Vector3Int.right}
                    },
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
            return new []
            {
                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/LMG - Build",
                    new [,,]
                    {
                        {{"Ships/Weapons/Projectiles/LMG (0, 0, 0)"}},
                        {{"Ships/Weapons/Projectiles/LMG (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "Light Machine Gun",
                    "A lighter machine gun firing smaller rounds at a higher rate of fire. Does poorly against shields and armor, but the higher projectile speed means higher accuracy at longer ranges.",
                    200, new Cost(0, 0, 0, 10, 0), false, new [] {new Connector {Direction = Vector3Int.down}},
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 0, 200, 1, 6000,
                    new ProjectileEnergies(2050, 463, 0), 15000, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HMG - Build",
                    new [,,]
                    {
                        {{"Ships/Weapons/Projectiles/HMG (0, 0, 0)"}},
                        {{"Ships/Weapons/Projectiles/HMG (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "Heavy Machine Gun",
                    "A heavier machine gun boasting higher damage at the cost of power consumption, ammo storage, rate of fire, and accuracy.",
                    400, new Cost(0, 0, 0, 58, 0), false, new [] {new Connector {Direction = Vector3Int.down } },
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 0, 400, 1, 3000,
                    new ProjectileEnergies(17088, 924, 0), 6800, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Chain Gun - Build",
                    new [,,]
                    {
                        {{"Ships/Weapons/Projectiles/Chain Gun (0, 0, 0)"}},
                        {{"Ships/Weapons/Projectiles/Chain Gun (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "Chain Gun",
                    "This weapon has six barrels that rotate rapidly. It uses energy from the ship's power source to reload the weapon chambers, rather than diverting energy from the combustion of the projectile. The result is an extremely high rate of fire with negligible energy drain from the projectiles.",
                    750, new Cost(0, 0, 0, 112, 0), false, new [] {new Connector {Direction = new Vector3Int(0, 0, 1)}},
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 0, 750, 2, 12000,
                    new ProjectileEnergies(56448, 11576, 0), 27000, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/AA Gun - Build",
                    new [,,]
                    {
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (0, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (0, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (1, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (1, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (2, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (2, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (3, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (3, 1, 0)"}
                        },
                    },
                    new []
                    {
                        Vector3Int.zero,
                        Vector3Int.up,
                        new Vector3Int(1, 1, 0),
                        new Vector3Int(2, 1, 0),
                        new Vector3Int(3, 1, 0),
                        Vector3Int.right,
                        new Vector3Int(2, 0, 0),
                        new Vector3Int(3, 1, 0)
                    }, "Flak Cannon",
                    "A four-barreled, high rate-of-fire cannon that fires explosive shells filled with shrapnel. These projectiles only detonate when they come in proximity of enemy strike craft. Useless against larger ships.",
                    750, new Cost(0, 0, 2750, 4750, 0), true, new []
                    {
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.zero),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.right),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.up),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, new Vector3Int(1, 1, 0))
                    },
                    new []
                    {
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, Vector3Int.up)
                    }, 4, 1000, 2, 5000, new ProjectileEnergies(1920643, 752166, 18328160), 11250, 5),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/HE Cannon - Build",
                    new [,,]
                    {
                        {{"Ships/Weapons/Projectiles/HE Cannon (0, 0, 0)"}},
                        {{"Ships/Weapons/Projectiles/HE Cannon (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "HE Cannon",
                    "A gun firing high-explosive (HE) projectiles. These guns fire bullets filled with acids or powders that ignite on impact, creating large explosions on impact.",
                    1000, new Cost(0, 0, 1243, 6757, 0), false, new [] {new Connector {Direction = Vector3Int.down}},
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 6, 2000, 4, 2000,
                    new ProjectileEnergies(20270460, 2172045, 14504000), 4500, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Incendiary Cannon - Build",
                    new [,,]
                    {
                        {{"Ships/Weapons/Projectiles/Incendiary Cannon (0, 0, 0)"}},
                        {{"Ships/Weapons/Projectiles/Incendiary Cannon (1, 0, 0)"}}
                    },
                    new [] {Vector3Int.zero, Vector3Int.right}, "Incendiary Cannon",
                    "A gun firing a shell filled with a combustible phosphorus powder. The powder is spread on contact, and subsequently ignites, burning anyone or anything caught in the cloud. Does high damage against flesh and hull, a small bit of damage to shields, but no damage to armor.",
                    1000, new Cost(0, 0, 3000, 7000, 0), false, new [] {new Connector {Direction = Vector3Int.down}},
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 8, 2500, 6, 2500,
                    new ProjectileEnergies(19868750, 2210630, 30000000), 4500, 1),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Amber Gun - Build",
                    new [,,]
                    {
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (0, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (0, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (1, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (1, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (2, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (2, 1, 0)"}
                        },
                        {
                            {"Ships/Weapons/Projectiles/AA Gun (3, 0, 0)"},
                            {"Ships/Weapons/Projectiles/AA Gun (3, 1, 0)"}
                        },
                    },
                    new []
                    {
                        Vector3Int.zero,
                        Vector3Int.up,
                        new Vector3Int(1, 1, 0),
                        new Vector3Int(2, 1, 0),
                        new Vector3Int(3, 1, 0),
                        Vector3Int.right,
                        new Vector3Int(2, 0, 0),
                        new Vector3Int(3, 1, 0)
                    }, "Amber Quarantine Gun",
                    "A gun that fires a projectile which erupts with an inert gas, covering but not harming a 10-m radius with a thick smog. After 5s, the smog solidifies into a substance harder than steel.",
                    1750, new Cost(2000, 0, 3000, 72500, 0), true, new []
                    {
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.zero),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.right),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, Vector3Int.up),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat}, new Vector3Int(1, 1, 0))
                    },
                    new []
                    {
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}),
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, Vector3Int.up)
                    }, 8, 4000, 6, 1500, new ProjectileEnergies(12500000, 803865, 0), 3500, 10),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Tracer Gun - Build",
                    new [,,]{{{"Ships/Weapons/Projectiles/Tracer Gun - Build"}}},
                    new [] {Vector3Int.zero}, "Tracer Gun",
                    "A gun that fires a projectile that emits a moderately-strong radio frequency. On a hit, it embeds itself in the hull or skin of an enemy, allowing your other weapons to more accurately target that unit. Negates the cloaking units of any enemies hit.",
                    1000, new Cost(0, 0.03f, 0, 320, 0), false, new [] {new Connector {Direction = new Vector3Int(0, 0, 1) } },
                    new [] {new ExclusionVector(new [] {ExclusionVectorDirections.RightLine})}, 3, 5000, 2, 500,
                    new ProjectileEnergies(625000, 500, 0), 1000, 0),

                new WeaponBlueprint(WeaponTypes.Projectile, "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon - Build",
                    new [,,]
                    {
                        {
                            {
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (0, 0, 0)",
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (0, 0, 1)"
                            },
                            {
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (0, 1, 0)",
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (0, 1, 1)",
                            }
                        },
                        {
                            {
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (1, 0, 0)",
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (1, 0, 1)"
                            },
                            {
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (1, 1, 0)",
                                "Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (1, 1, 1)"
                            }
                        },
                        {
                            {"Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (2, 0, 0)", null},
                            {"Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (2, 1, 0)", null}
                        },
                        {
                            {"Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (3, 0, 0)", null},
                            {"Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon (3, 1, 0)", null}
                        },
                    },
                    new []
                    {
                        new Vector3Int(0, 0, 0),
                        new Vector3Int(0, 1, 0),
                        new Vector3Int(1, 0, 0),
                        new Vector3Int(1, 1, 0),
                        new Vector3Int(0, 0, 1),
                        new Vector3Int(0, 1, 1),
                        new Vector3Int(1, 1, 1),
                        new Vector3Int(1, 0, 1),
                        new Vector3Int(2, 0, 1),
                        new Vector3Int(2, 1, 1),
                        new Vector3Int(3, 0, 1),
                        new Vector3Int(3, 1, 1),
                    }, "Depleted-Uranium Slug Cannon",
                    "A large cannon that fires hyper-dense slugs of an alloy composed of tungsten and uranium-238 (aka, depleted uranium). Requires a large explosive force to fire the projectile. Less accurate than the other weapons, but does high damage against hull/flesh and armor.",
                    2000000, new Cost(0, 0, 0, 2500000, 0), true, new []
                    {
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat, Materials.Air}, new Vector3Int(0, 0, 0)),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat, Materials.Air}, new Vector3Int(0, 1, 0)),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat, Materials.Air}, new Vector3Int(1, 0, 0)),
                        new Connector(new Vector3Int(0, 0, -1), new []{Materials.Power, Materials.Heat, Materials.Air}, new Vector3Int(1, 1, 0)),
                    },
                    new []
                    {
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, new Vector3Int(0, 2, 0)),
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, new Vector3Int(0, 3, 0)),
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, new Vector3Int(0, 2, 1)),
                        new ExclusionVector(new [] {ExclusionVectorDirections.RightLine}, new Vector3Int(0, 3, 1))
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
            return new[]
            {
                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel",
                new [,,]{{{ "Ships/Miscellanious/Decorative/Panel" }}},
                    new[] {Vector3Int.zero}, "Panel",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new Connector(new Vector3Int(0, 0, -1), new Materials[0]),
                        new Connector(new Vector3Int(0, 0, 1), new Materials[0]),
                        new Connector(Vector3Int.down, new Materials[0]),
                        new Connector(Vector3Int.up, new Materials[0]),
                        new Connector(Vector3Int.right, new Materials[0]),
                        new Connector(Vector3Int.left, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Panel 2",
                    new [,,] {{{ "Ships/Miscellanious/Decorative/Panel 2" }}},
                    new[] {Vector3Int.zero}, "Panel 2",
                    "A basic panel. Provides no armor or hull, merely helps to connect pieces of your ship together.",
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new Connector(new Vector3Int(0, 0, -1), new Materials[0]),
                        new Connector(new Vector3Int(0, 0, 1), new Materials[0]),
                        new Connector(Vector3Int.down, new Materials[0]),
                        new Connector(Vector3Int.up, new Materials[0]),
                        new Connector(Vector3Int.right, new Materials[0]),
                        new Connector(Vector3Int.left, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Crossbeam Connector",
                    new [,,] {{{ "Ships/Miscellanious/Decorative/Crossbeam Connector" }}},
                    new[] {Vector3Int.zero}, "Crossbeam Connector",
                    "A basic connector. Provides no armor or hull. Holds pieces of your ship together on the top or bottom.",
                    0, 1000, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new Connector(Vector3Int.down, new Materials[0]),
                        new Connector(Vector3Int.up, new Materials[0])
                    },
                    new ExclusionVector[] { }),

                new DecorativeModuleBlueprint("Ships/Miscellanious/Decorative/Weapon Mount",
                    new[,,]{{{ "Ships/Miscellanious/Decorative/Weapon Mount" }}},
                    new[] {Vector3Int.zero},
                    "Weapon Mount",
                    "A simple weapon mount that allows small weapons to be attached to the underside of panels or wings.",
                    0, 250, new Cost(0, 0, 0, 1, 0), false, new[]
                    {
                        new Connector(new Vector3Int(0, 0, 1), new[] {Materials.Power}),
                        new Connector(Vector3Int.down, new[] {Materials.Power}),
                        new Connector(Vector3Int.up, new[] {Materials.Power})
                    },
                    new ExclusionVector[] { })
            };
        }
        private static HallwayModuleBlueprint[] GetHallwayModules()
        {
            return new []
            {
                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NS Hallway - Build",
                    new [,,] {{{ "Ships/Miscellanious/Hallways/NS Hallway - Build" }}},
                    new [] { Vector3Int.zero},
                    "NS Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new []
                    {
                        new Connector(Vector3Int.left, new Materials[0]),
                        new Connector(Vector3Int.right, new Materials[0]),
                        new Connector(Vector3Int.up,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new Connector(Vector3Int.down,  new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new Connector(new Vector3Int(0, 0, 1), new Materials[0]),
                        new Connector(new Vector3Int(0, 0, -1), new Materials[0]),
                    },
                    new ExclusionVector[] { }),

                new HallwayModuleBlueprint("Ships/Miscellanious/Hallways/NW-SE Hallway - Build",
                    new[,,]{{{ "Ships/Miscellanious/Hallways/NW-SE Hallway - Build" }}},
                    new [] { Vector3Int.zero, Vector3Int.up},
                    "NW-SE Hallway",
                    "A basic hallway. Provides no armor or hull. Holds pieces of your ship together, and can hold an atmosphere.",
                    0, 1000, new Cost(0, 1, 0, 1, 0), false, new []
                    {
                        new Connector(Vector3Int.right, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}),
                        new Connector(new Vector3Int(0, 0, 1), new Materials[0]),
                        new Connector(new Vector3Int(0, 0, -1), new Materials[0]),
                        new Connector(Vector3Int.left, new []{Materials.Power, Materials.Heat, Materials.Air, Materials.Water, Materials.Waste}, Vector3Int.up),
                        new Connector(new Vector3Int(0, 0, 1), new Materials[0], Vector3Int.up),
                        new Connector(new Vector3Int(0, 0, -1), new Materials[0], Vector3Int.up),
                    },
                    new ExclusionVector[] { }),
            };
        }

        #endregion
    }
}

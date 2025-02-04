﻿using System;
using System.Collections.Generic;
using Assets.Data;
using Assets.Ships;
using Assets.Ships.Modules;
using Assets.Ships.Modules.Command;
using Assets.Ships.Modules.Miscellanious;
using Assets.Ships.Modules.Weapons;
using Assets.Utils.Extensions;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class MenuDataManager
    {
        private ModuleBlueprint[] _blueprints;

        public MenuData Get()
        {
            _blueprints = ModuleBlueprintsManager.Generate();

            return new MenuData(new List<ToggleData>
            {
                GetCommandCentres(),
                GetWeapons(),
                GetMiscellanious()
            });
        }

        #region Command Centres

        private ToggleData GetCommandCentres()
        {
            return new ToggleData
            (
                "Ships/Control Centres Icon", "Control Centres", true, new MenuData
                (
                    new List<ToggleData>
                    {
                        GetCockpits()
                    }
                ), Modals.BuildMenu.CommandModulesModalData
            );
        }
        private ToggleData GetCockpits()
        {
            var cockpits = new List<ToggleData>
            {
                new ToggleData("Ships/Control Centres/Small Ship/Basic Cockpit - Build",
                    "Basic Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Basic Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Advanced Cockpit - Build",
                    "Advanced Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Advanced Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Heavy Cockpit - Build",
                    "Heavy Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Heavy Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    "Advanced Heavy Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Advanced Heavy Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    "Tactical Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Tactical Cockpit") as CockpitModuleBlueprint
                    )
                )
            };

            return new ToggleData
            (
                "Ships/Control Centres/Cockpits Icon", "Small Ships", true, new MenuData(cockpits),
                Modals.BuildMenu.CockpitModalData
            );
        }

        #endregion

        #region Weapons

        private ToggleData GetWeapons()
        {
            return new ToggleData
            (
                "Ships/Weapons Icon", "Weapons", true, new MenuData
                (
                    new List<ToggleData>
                    {
                        GetProjectiles()
                    }
                )
            );
        }

        private ToggleData GetProjectiles()
        {
            Func<List<DetailsField>> getFields = () => new List<DetailsField>()
            {
                new DetailsField("Rate of Fire", "ShipBuild/Rate of Fire Icon", "", "Range", "ShipBuild/Range Icon", ""),
                new DetailsField("Ammo Storage", "ShipBuild/Ammo Storage Icon", "", "Damage Radius", "ShipBuild/Splash Damage Icon", ""),
                new DetailsFieldThird("Kinetic Energy", "Common/Kinetic Energy Symbol", "", "Thermal Energy", "Common/Thermal Energy Symbol", "", "Chemical Potential Energy", "Common/Chemical Potential Energy Symbol", "")
            };

            WeaponBlueprint lmgbp = FindByName("Light Machine Gun") as WeaponBlueprint;
            var lmg = new ToggleData("Ships/Weapons/Projectiles/LMG - Build",
                "Light Machine Gun", false, new DetailsMenuData(getFields(), lmgbp));
            AssignProjectileFieldValues(lmg.ChildMenu as DetailsMenuData, lmgbp);

            WeaponBlueprint hmgbp = FindByName("Heavy Machine Gun") as WeaponBlueprint;
            var hmg = new ToggleData("Ships/Weapons/Projectiles/HMG - Build",
                "Heavy Machine Gun", false, new DetailsMenuData(getFields(), hmgbp));
            AssignProjectileFieldValues(hmg.ChildMenu as DetailsMenuData, hmgbp);

            WeaponBlueprint chaingunBlueprint = FindByName("Chain Gun") as WeaponBlueprint;
            var chaingun = new ToggleData("Ships/Weapons/Projectiles/Chain Gun - Build",
                "Chain Gun", false, new DetailsMenuData(getFields(), chaingunBlueprint));
            AssignProjectileFieldValues(chaingun.ChildMenu as DetailsMenuData, chaingunBlueprint);

            WeaponBlueprint flakBlueprint = FindByName("Flak Cannon") as WeaponBlueprint;
            var flak = new ToggleData("Ships/Weapons/Projectiles/AA Gun - Build",
                "Flak Cannon", false, new DetailsMenuData(getFields(), flakBlueprint));
            AssignProjectileFieldValues(flak.ChildMenu as DetailsMenuData, flakBlueprint);

            WeaponBlueprint heCannonBlueprint = FindByName("HE Cannon") as WeaponBlueprint;
            var heCannon = new ToggleData("Ships/Weapons/Projectiles/HE Cannon - Build",
                "High-Explosive Cannon", false, new DetailsMenuData(getFields(), heCannonBlueprint));
            AssignProjectileFieldValues(heCannon.ChildMenu as DetailsMenuData, heCannonBlueprint);

            WeaponBlueprint incendiaryCannonBlueprint = FindByName("Incendiary Cannon") as WeaponBlueprint;
            var incendiary = new ToggleData("Ships/Weapons/Projectiles/Incendiary Cannon - Build",
                "Incendiary Cannon", false, new DetailsMenuData(getFields(), incendiaryCannonBlueprint));
            AssignProjectileFieldValues(incendiary.ChildMenu as DetailsMenuData, incendiaryCannonBlueprint);

            WeaponBlueprint amberGunBlueprint = FindByName("Amber Quarantine Gun") as WeaponBlueprint;
            var amberGun = new ToggleData("Ships/Weapons/Projectiles/Amber Gun - Build",
                "Amber Quarantine Gun", false, new DetailsMenuData(getFields(), amberGunBlueprint));
            AssignProjectileFieldValues(amberGun.ChildMenu as DetailsMenuData, amberGunBlueprint);

            WeaponBlueprint tracerBlueprint = FindByName("Tracer Gun") as WeaponBlueprint;
            var tracer = new ToggleData("Ships/Weapons/Projectiles/Tracer Gun",
                "Tracer Gun", false, new DetailsMenuData(getFields(), tracerBlueprint));
            AssignProjectileFieldValues(tracer.ChildMenu as DetailsMenuData, tracerBlueprint);

            WeaponBlueprint duscBlueprint = FindByName("Depleted-Uranium Slug Cannon") as WeaponBlueprint;
            var dusc = new ToggleData("Ships/Weapons/Projectiles/Depleted-Uranium Slug Cannon - Build",
                "Depleted-Uranium Slug Cannon", false, new DetailsMenuData(getFields(), duscBlueprint));
            AssignProjectileFieldValues(dusc.ChildMenu as DetailsMenuData, duscBlueprint);

            return new ToggleData
            (
                "Ships/Weapons/Projectiles Icon", "Projectiles", true, new MenuData(new List<ToggleData>
                {
                    lmg,
                    hmg,
                    chaingun,
                    flak,
                    heCannon,
                    incendiary,
                    amberGun,
                    tracer,
                    dusc
                })
            );
        }

        private void AssignProjectileFieldValues(DetailsMenuData menu, WeaponBlueprint blueprint)
        {
            //TODO: Reassign these
            menu.DetailsFields[0].Value1 = blueprint.RateOfFire.ToString();
            menu.DetailsFields[1].Value1 = blueprint.AmmoStorage.ToString();
            menu.DetailsFields[1].Value2 = blueprint.DamageRadius.ToString();
            menu.DetailsFields[2].Value1 = blueprint.ProjectileEnergies.Kinetic.ToSiUnit("J");
            menu.DetailsFields[2].Value2 = blueprint.ProjectileEnergies.Thermal.ToSiUnit("J");
            (menu.DetailsFields[2] as DetailsFieldThird).Value3 = blueprint.ProjectileEnergies.ChemicalPotential.ToSiUnit("J");
        }
        #endregion

        #region Miscellanious

        private ToggleData GetMiscellanious()
        {
            return new ToggleData
            (
                "Ships/Misc Icon", "Miscellanious", true, new MenuData
                (
                    new List<ToggleData>
                    {
                        GetDecorative(),
                        GetHallway()
                    }
                )
            );
        }
        private ToggleData GetDecorative()
        {
            var decoratives = new List<ToggleData>
            {
                new ToggleData("Ships/Miscellanious/Decorative/Panel",
                    "Panel", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Panel") as DecorativeModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Decorative/Panel 2",
                    "Panel", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Panel 2") as DecorativeModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Decorative/Crossbeam Connector",
                    "Crossbeam Connector", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Crossbeam Connector") as DecorativeModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Decorative/Weapon Mount",
                    "Weapon Mount", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("Weapon Mount") as DecorativeModuleBlueprint
                    )
                ),
            };

            return new ToggleData
            (
                "Ships/Miscellanious/Decorative Icon", "Decorative", true, new MenuData(decoratives)
            );
        }
        private ToggleData GetHallway()
        {
            var hallways = new List<ToggleData>
            {
                new ToggleData("Ships/Miscellanious/Hallways/NS Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("NS Hallway") as HallwayModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Hallways/NW-SE Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        FindByName("NW-SE Hallway") as HallwayModuleBlueprint
                    )
                ),
            };

            return new ToggleData
            (
                "Ships/Miscellanious/Hallway Icon", "Hallways", true, new MenuData(hallways)
            );
        }

        #endregion

        private ModuleBlueprint FindByName(string name)
        {
            ModuleBlueprint item = null;

            foreach(var i in _blueprints)
                if (i.Name == name)
                    item = i;

            return item;
        }
    }
}

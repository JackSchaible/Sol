using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Data;
using Assets.Ships;
using Assets.Ships.Modules.Miscellanious;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class MenuDataManager
    {
        private ModuleBlueprint[] _blueprints;

        public MenuData Get()
        {
            _blueprints = new ModuleBlueprintsManager().Get();

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
                        _blueprints.First(x => x.Name == "Basic Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Advanced Cockpit - Build",
                    "Advanced Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Advanced Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Heavy Cockpit - Build",
                    "Heavy Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Heavy Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Advanced Heavy Cockpit - Build",
                    "Advanced Heavy Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Advanced Heavy Cockpit") as CockpitModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Control Centres/Small Ship/Tactical Cockpit - Build",
                    "Tactical Cockpit", false,
                    new CommandModuleDetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Tactical Cockpit") as CockpitModuleBlueprint
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
                new DetailsFieldQw("Damage vs Flesh", "ShipBuild/Damage Vs Flesh Icon", "", "Damage vs Hull", "ShipBuild/Damage Vs Hull Icon", "", "Damage vs Armor", "ShipBuild/Damage Vs Armor Icon", "", "Damage vs Shields", "ShipBuild/Damage Vs Shields Icon", "")
            };

            WeaponBlueprint lmgbp = _blueprints.First(x => x.Name == "Light Machine Gun") as WeaponBlueprint;
            var lmg = new ToggleData("Ships/Weapons/Projectiles/LMG - Build",
                "Light Machine Gun", false, new DetailsMenuData(getFields(), lmgbp));
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[0].Value1 = lmgbp.RateOfFire.ToString();
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[0].Value2 = lmgbp.Range.ToString();
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[1].Value1 = lmgbp.AmmoStorage.ToString();
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[1].Value2 = lmgbp.DamageRadius.ToString();
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[2].Value1 = lmgbp.Damage.VsFlesh.ToString();
            (lmg.ChildMenu as DetailsMenuData).DetailsFields[2].Value2 = lmgbp.Damage.VsHull.ToString();
            ((lmg.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value3 = lmgbp.Damage.VsArmor.ToString();
            ((lmg.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value4 = lmgbp.Damage.VsShields.ToString();

            WeaponBlueprint hmgbp = _blueprints.First(x => x.Name == "Heavy Machine Gun") as WeaponBlueprint;
            var hmg = new ToggleData("Ships/Weapons/Projectiles/HMG - Build",
                "Heavy Machine Gun", false, new DetailsMenuData(getFields(), hmgbp));
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[0].Value1 = hmgbp.RateOfFire.ToString();
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[0].Value2 = hmgbp.Range.ToString();
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[1].Value1 = hmgbp.AmmoStorage.ToString();
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[1].Value2 = hmgbp.DamageRadius.ToString();
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[2].Value1 = hmgbp.Damage.VsFlesh.ToString();
            (hmg.ChildMenu as DetailsMenuData).DetailsFields[2].Value2 = hmgbp.Damage.VsHull.ToString();
            ((hmg.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value3 = hmgbp.Damage.VsArmor.ToString();
            ((hmg.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value4 = hmgbp.Damage.VsShields.ToString();

            WeaponBlueprint amberGunBlueprint = _blueprints.First(x => x.Name == "Amber Quarantine Gun") as WeaponBlueprint;
            var amberGun = new ToggleData("Ships/Weapons/Projectiles/Amber Gun - Build",
                "Amber Quarantine Gun", false, new DetailsMenuData(getFields(), amberGunBlueprint));
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[0].Value1 = amberGunBlueprint.RateOfFire.ToString();
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[0].Value2 = amberGunBlueprint.Range.ToString();
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[1].Value1 = amberGunBlueprint.AmmoStorage.ToString();
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[1].Value2 = amberGunBlueprint.DamageRadius.ToString();
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[2].Value1 = amberGunBlueprint.Damage.VsFlesh.ToString();
            (amberGun.ChildMenu as DetailsMenuData).DetailsFields[2].Value2 = amberGunBlueprint.Damage.VsHull.ToString();
            ((amberGun.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value3 = amberGunBlueprint.Damage.VsArmor.ToString();
            ((amberGun.ChildMenu as DetailsMenuData).DetailsFields[2] as DetailsFieldQw).Value4 = amberGunBlueprint.Damage.VsShields.ToString();

            return new ToggleData
            (
                "Ships/Weapons/Projectiles Icon", "Projectiles", true, new MenuData(new List<ToggleData>
                {
                    lmg,
                    hmg,
                    amberGun
                })
            );
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
                new ToggleData("Ships/Miscellanious/Decorative/Panel - Build",
                    "Panel", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Panel") as DecorativeModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Decorative/Panel 2 - Build",
                    "Panel", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Panel 2") as DecorativeModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Decorative/Crossbeam Connector - Build",
                    "Crossbeam Connector", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "Crossbeam Connector") as DecorativeModuleBlueprint
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
                new ToggleData("Ships/Miscellanious/Hallways/EW Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "EW Hallway") as HallwayModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Hallways/NS Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "NS Hallway") as HallwayModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Hallways/NE-SW Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "NE-SW Hallway") as HallwayModuleBlueprint
                    )
                ),
                new ToggleData("Ships/Miscellanious/Hallways/NW-SE Hallway - Build",
                    "Hallway", false,
                    new DetailsMenuData
                    (
                        new List<DetailsField>(),
                        _blueprints.First(x => x.Name == "NW-SE Hallway") as HallwayModuleBlueprint
                    )
                ),
            };

            return new ToggleData
            (
                "Ships/Miscellanious/Hallway Icon", "Hallways", true, new MenuData(hallways)
            );
        }

        #endregion
    }
}

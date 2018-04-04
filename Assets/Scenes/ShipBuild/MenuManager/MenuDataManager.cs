using System.Collections.Generic;
using System.Linq;
using Assets.Data;
using Assets.Ships;

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
            var bp = _blueprints.First(x => x.Name == "Basic Cockpit");
            var c = bp as CockpitModuleBlueprint;

            CommandModuleBlueprint bpc = _blueprints.First(x => x.Name == "Basic Cockpit") as CockpitModuleBlueprint;
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
    }
}

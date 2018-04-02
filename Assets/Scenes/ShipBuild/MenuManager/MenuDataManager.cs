using System.Collections.Generic;
using Assets.Data;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    class MenuDataManager
    {
        public MenuData Get()
        {
            return new MenuData(new List<ToggleData>
            {
                new ToggleData
                (
                    "Ships/Control Centres Icon", "Control Centres", true, new MenuData
                    (
                        new List<ToggleData>
                        {
                            new ToggleData
                            (
                                "Ships/Control Centres/Cockpits Icon", "Small Ships", true, 
                                new MenuData
                                (
                                    new List<ToggleData>
                                    {
                                        new ToggleData("Ships/Control Centres/Small Ship/Basic Cockpit - Build", "Basic Cockpit", false, 
                                            new CommandModuleDetailsMenuData
                                            (
                                                new List<DetailsField>(),
                                                "A very basic cockpit, containing the essentials for flying a small ship; attitude, yaw, roll, and thruster control, as well as communications, life support, and basic navigation and targeting algorithms.",
                                                25, 25, 500, 200, 8, 1
                                            )
                                        )
                                    }
                                ),
                                Modals.BuildMenu.CockpitModalData
                            )
                        }
                    ), Modals.BuildMenu.CommandModulesModalData
                )
            });
        }
    }
}

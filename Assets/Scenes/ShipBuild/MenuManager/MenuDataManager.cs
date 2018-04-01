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
                                            new DetailsMenuData
                                            (
                                                new List<DetailsField>
                                                {
                                                    new DetailsField("Modules", "ShipBuild/Modules Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Life Support", "Ships/Life Support Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Personnel", "ShipBuild/Personnel Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Weight", "ShipBuild/Weight Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Power", "Ships/Power Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Cost", "Common/Currency Icon", DetailsField.DisplayWidths.Full),
                                                }
                                            )
                                        )
                                    }
                                ),
                                Modals.BuildMenu.CockpitModalData
                            )
                        }
                    ), Modals.BuildMenu.CommandModulesModalData
                ),

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
                                            new DetailsMenuData
                                            (
                                                new List<DetailsField>
                                                {
                                                    new DetailsField("Modules", "ShipBuild/Modules Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Life Support", "Ships/Life Support Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Personnel", "ShipBuild/Personnel Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Weight", "ShipBuild/Weight Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Power", "Ships/Power Icon", DetailsField.DisplayWidths.Half),
                                                    new DetailsField("Cost", "Common/Currency Icon", DetailsField.DisplayWidths.Full),
                                                }
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

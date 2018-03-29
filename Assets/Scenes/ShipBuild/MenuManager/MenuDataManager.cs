using System.Collections.Generic;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    class MenuDataManager
    {
        public MenuData Get()
        {
            return new MenuData(new List<ToggleData>
            {
                new ToggleData("", "", false, null)
            });
        }
    }
}

using UnityEngine;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class MenuToggle
    {
        public GameObject GameObject { get; set; }
        public Menu Menu { get; set; }

        public MenuToggle()
        {
            
        }

        public MenuToggle(GameObject gameObject, Menu menu)
        {
            GameObject = gameObject;
            Menu = menu;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class Menu
    {
        public GameObject GameObject { get; set; }
        public List<MenuToggle> MenuToggles { get; set; }

        public Menu()
        {
            
        }

        public Menu(GameObject gameObject, List<MenuToggle> menuToggles)
        {
            GameObject = gameObject;
            MenuToggles = menuToggles;
        }
    }
}

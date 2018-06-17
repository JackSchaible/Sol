using UnityEngine;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class MenuToggle
    {
        public GameObject GameObject { get; set; }
        public Menu Menu { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public MenuToggle()
        {
            
        }

        public MenuToggle(GameObject gameObject, Menu menu, string title, string description)
        {
            GameObject = gameObject;
            Menu = menu;
            Title = title;
            Description = description;
        }
    }
}

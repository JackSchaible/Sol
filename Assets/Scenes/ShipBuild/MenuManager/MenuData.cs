using System.Collections.Generic;
using Assets.Data;

namespace Assets.Scenes.ShipBuild.MenuManager
{
    internal class MenuData
    {
        public List<ToggleData> MenuDatas { get; set; }

        public MenuData(List<ToggleData> menuDatas)
        {
            MenuDatas = menuDatas;
        }
    }

    internal class ToggleData
    {
        public string Image { get; set; }
        public string Text { get; set; }
        public bool HasPopup { get; set; }
        public ModalData PopupData { get; set; }
        public MenuData ChildMenu { get; set; }

        public ToggleData(string image, string text, bool hasPopup, MenuData childMenu, ModalData popupData = null)
        {
            Image = image;
            Text = text;
            HasPopup = hasPopup;
            PopupData = popupData;
            ChildMenu = childMenu;
        }
    }

    internal class DetailsMenuData : MenuData
    {
        public List<DetailsField> DetailsFields { get; set; }

        public DetailsMenuData(List<DetailsField> detailsFields) : base(null)
        {
            DetailsFields = detailsFields;
        }
    }

    internal class DetailsField
    {
        public enum DisplayWidths
        {
            Quarter,
            Half,
            Full
        }

        public string Name { get; set; }
        public string Icon { get; set; }
        public DisplayWidths DisplayWidth { get; set; }

        public DetailsField(string name, string icon, DisplayWidths displayWidth)
        {
            Name = name;
            Icon = icon;
            DisplayWidth = displayWidth;
        }
    }
}

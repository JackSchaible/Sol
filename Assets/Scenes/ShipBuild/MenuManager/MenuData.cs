using System.Collections.Generic;
using Assets.Data;
using Assets.Ships;

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
        public ModuleBlueprint Blueprint { get; set; }

        public DetailsMenuData(List<DetailsField> detailsFields, ModuleBlueprint blueprint)
            : base(null)
        {
            DetailsFields = detailsFields;
            Blueprint = blueprint;
        }
    }

    internal class CommandModuleDetailsMenuData : DetailsMenuData
    {
        public CommandModuleDetailsMenuData(List<DetailsField> detailsFields, CommandModuleBlueprint cmb)
            :base (detailsFields, cmb) { }
    }

    internal class DetailsField
    {
        public string Name1 { get; set; }
        public string Icon1 { get; set; }
        public string Value1 { get; set; }
        public string Name2 { get; set; }
        public string Icon2 { get; set; }
        public string Value2 { get; set; }

        public DetailsField(string name1, string icon1, string value1, string name2, string icon2, string value2)
        {
            Name1 = name1;
            Icon1 = icon1;
            Value1 = value1;
            Name2 = name2;
            Icon2 = icon2;
            Value2 = value2;
        }
    }

    internal class DetailsFieldThird : DetailsField
    {
        public string Name3 { get; set; }
        public string Icon3 { get; set; }
        public string Value3 { get; set; }

        public DetailsFieldThird(string name1, string icon1, string value1, string name2, string icon2, string value2, string name3, string icon3, string value3)
            : base(name1, icon1, value1, name2, icon2, value2)
        {
            Name3 = name3;
            Icon3 = icon3;
            Value3 = value3;
        }
    }

    internal class DetailsFieldQw : DetailsField
    {
        public string Name3 { get; set; }
        public string Icon3 { get; set; }
        public string Value3 { get; set; }
        public string Name4 { get; set; }
        public string Icon4 { get; set; }
        public string Value4 { get; set; }

        public DetailsFieldQw(string name1, string icon1, string value1, string name2, string icon2, string value2, string name3, string icon3, string value3, string name4, string icon4, string value4)
            : base(name1, icon1, value1, name2, icon2, value2)
        {
            Name3 = name3;
            Icon3 = icon3;
            Value3 = value3;
            Name4 = name4;
            Icon4 = icon4;
            Value4 = value4;
        }
    }
}

namespace Assets.Data
{
    public static class Modals
    {
        public static class BuildMenu
        {
            public static ModalData CommandModulesModalData = new ModalData(ModalTypes.Info, "Command Modules", "Command modules must be placed first, before any other module.");
            public static ModalData CockpitModalData = new ModalData(ModalTypes.Info, "Cockpit Modules", "You may only place one Small Ship module. If you select a different type of command module, you will be unable to place a Small Ship module. You may not place other modules above or on the same plane as the cockpit module.");
        }
    }

    public class ModalData
    {
        public ModalTypes ModalType { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public ModalData()
        {

        }

        public ModalData(ModalTypes modalType, string title, string text)
        {
            ModalType = modalType;
            Title = title;
            Text = text;
        }
    }
}

public enum ModalTypes
{
    Success,
    Info,
    Error,
    Input
}

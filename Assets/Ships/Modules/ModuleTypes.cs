namespace Assets.Ships
{
    public enum ModuleTypes
    {
        Weapon,
        Armor,
        Shield,
        Engine,
        FTLEngine,
        Power,
        LifeSupport,
        Marine,
        StrikeCraft,
        Engineering,
        ControlCentre,
        Miscellanious
    }

    public static class ControlCentreTypes
    {
        public const string SmallShip = "Small Ship";
    }

    public static class WeaponTypes
    {
        public const string Projectile = "Projectile";
    }

    public static class MiscellaniousTypes
    {
        public const string Decorative = "Decorative";
    }
}

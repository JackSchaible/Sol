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
        Other
    }

    public enum WeaponTypes
    {
        Projectile,
        Missile,
        EMCoil,
        Laser,
        Plasma,
        Chemical,
        Biological,
        Nuclear,
        Other
    }

    public static class ControlCentreTypes
    {
        public const string SmallShip = "Small Ship";
    }
}

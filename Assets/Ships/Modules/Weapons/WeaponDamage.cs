namespace Assets.Ships.Weapons
{
    public class WeaponDamage
    {
        public int VsFlesh { get; set; }
        public int VsHull { get; set; }
        public int VsArmor { get; set; }
        public int VsShields { get; set; }

        public WeaponDamage()
        {
            
        }

        public WeaponDamage(int vsFlesh, int vsHull, int vsArmor, int vsShields)
        {
            VsFlesh = vsFlesh;
            VsHull = vsHull;
            VsArmor = vsArmor;
            VsShields = vsShields;
        }
    }
}

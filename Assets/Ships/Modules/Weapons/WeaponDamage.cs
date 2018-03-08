namespace Assets.Ships.Weapons
{
    public class WeaponDamage
    {
        public int VsHull { get; set; }
        public int VsArmor { get; set; }
        public int VsShields { get; set; }

        public WeaponDamage()
        {
            
        }

        public WeaponDamage(int vsHull, int vsArmor, int vsShields)
        {
            VsHull = vsHull;
            VsArmor = vsArmor;
            VsShields = vsShields;
        }
    }
}

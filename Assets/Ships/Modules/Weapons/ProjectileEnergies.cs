namespace Assets.Ships.Modules.Weapons
{
    public class ProjectileEnergies
    {
        public float Kinetic { get; set; }
        public float Thermal { get; set; }
        public float ChemicalPotential { get; set; }

        public ProjectileEnergies()
        {
            
        }

        public ProjectileEnergies(float kinetic, float thermal, float chemicalPotential)
        {
            Kinetic = kinetic;
            Thermal = thermal;
            ChemicalPotential = chemicalPotential;
        }
    }
}

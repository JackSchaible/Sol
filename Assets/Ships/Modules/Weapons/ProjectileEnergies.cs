namespace Assets.Ships.Modules.Weapons
{
    public struct ProjectileEnergies
    {
        public float Kinetic { get; set; }
        public float Thermal { get; set; }
        public float ChemicalPotential { get; set; }

        public ProjectileEnergies(float kinetic, float thermal, float chemicalPotential) : this()
        {
            Kinetic = kinetic;
            Thermal = thermal;
            ChemicalPotential = chemicalPotential;
        }
    }
}

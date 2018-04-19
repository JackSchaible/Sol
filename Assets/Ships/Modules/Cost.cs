namespace Assets.Ships.Modules
{
    public class Cost
    {
        public float Gasses { get; set; }
        public float LightMetals { get; set; }
        public float OrganicMinerals { get; set; }
        public float HeavyMetals { get; set; }
        public float FissileMaterials { get; set; }

        public Cost()
        {
            
        }

        public Cost(float gasses, float lightMetals, float organicMinerals, float heavyMetals, float fissileMaterials)
        {
            Gasses = gasses;
            LightMetals = lightMetals;
            OrganicMinerals = organicMinerals;
            HeavyMetals = heavyMetals;
            FissileMaterials = fissileMaterials;
        }
    }
}

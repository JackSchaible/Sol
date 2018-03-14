namespace Assets.Common.Utils
{
    public struct IntVector
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public IntVector(int x, int y, int z) : this()
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}

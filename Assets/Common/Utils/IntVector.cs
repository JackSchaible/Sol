using Assets.Ships.Modules;
using UnityEngine;

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

        public bool IsAdjacent(IntVector compare)
        {
            var isAdjacent = false;

            if (compare.X == X && compare.Z == Z)
                if (compare.Y == Y - 1 || compare.Y == Y + 1)
                    isAdjacent = true;

            if (compare.Y == Y && compare.Z == Z)
                if (compare.X == X - 1 || compare.X == X + 1)
                    isAdjacent = true;

            if (compare.X == X && compare.Y == Y)
                if (compare.Z == Z - 1 || compare.Z == Z + 1)
                    isAdjacent = true;

            return isAdjacent;
        }

        public static IntVector GetRelativeVector(Vector3 a, Vector3 b)
        {
            return new IntVector((int)((b.x - a.x) / 50), (int)((b.y - a.y) / 50), (int)((b.z - a.z) / 50));
        }

        public bool DoesExclusionVectorBlock(ExclusionVectors ev, IntVector other)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}}}", X, Y, Z);
        }
    }
}

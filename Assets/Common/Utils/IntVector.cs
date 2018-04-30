using System;
using Assets.Ships.Modules;
using UnityEngine;

namespace Assets.Common.Utils
{
    public struct IntVector
    {
        #region Properties

        #region Static Properties

        /// <summary>
        /// {0, 0, 0}
        /// </summary>
        public static IntVector Zero = new IntVector(0, 0, 0);
        /// <summary>
        /// {-1, 0, 0}
        /// </summary>
        public static IntVector Left = new IntVector(-1, 0, 0);
        /// <summary>
        /// {1, 0, 0}
        /// </summary>
        public static IntVector Right = new IntVector(1, 0, 0);
        /// <summary>
        /// {0, 1, 0}
        /// </summary>
        public static IntVector Up = new IntVector(0, 1, 0);
        /// <summary>
        /// {0, -1, 0}
        /// </summary>
        public static IntVector Down = new IntVector(0, -1, 0);
        /// <summary>
        /// {0, 0, -1}
        /// </summary>
        public static IntVector Backward = new IntVector(0, 0, -1);
        /// <summary>
        /// {0, 0, 1}
        /// </summary>
        public static IntVector Forward = new IntVector(0, 0, 1);

        #endregion

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        #endregion

        #region ctor

        public IntVector(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        #endregion

        #region Methods

        #region Static Methods

        public static IntVector operator +(IntVector a, IntVector b)
        {
            return new IntVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static bool operator ==(IntVector a, IntVector b)
        {
            return (a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }
        public static bool operator !=(IntVector a, IntVector b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Returns the IntVector represented by the Vector3, relative to Vector3.zero
        /// </summary>
        /// <param name="a">The vector to transform</param>
        /// <returns>The IntVector represented by the Vector3</returns>
        public static IntVector GetRelativeVector(Vector3 a)
        {
            return new IntVector((int)((a.x) / 50), (int)((a.y) / 50), (int)((a.z) / 50));
        }

        /// <summary>
        /// Returns the IntVector represented by a Vector3, relative to another Vector3
        /// </summary>
        /// <param name="a">The vector to transform</param>
        /// <param name="b">The reference vector to comapre to</param>
        /// <returns>The IntVector represented by the difference in Vector3's</returns>
        public static IntVector GetRelativeVector(Vector3 a, Vector3 b)
        {
            return new IntVector((int)((b.x - a.x) / 50), (int)((b.y - a.y) / 50), (int)(b.z - a.z));
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return string.Format("{{{0}, {1}, {2}}}", X, Y, Z);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            try
            {
                var intVector = (IntVector) obj;
                return X == intVector.X && Y == intVector.Y && Z == intVector.Z;
            }
            catch (InvalidCastException)
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        public IntVector SetZ(int z)
        {
            Z = z;
            return this;
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

        public IntVector Adjust(ConnectorDirections cp)
        {
            switch (cp)
            {
                case ConnectorDirections.Up:
                    return new IntVector(X, Y + 1, Z);

                case ConnectorDirections.Right:
                    return new IntVector(X + 1, Y, Z);

                case ConnectorDirections.Down:
                    return new IntVector(X, Y - 1, Z);

                case ConnectorDirections.Left:
                    return new IntVector(X - 1, Y, Z);

                case ConnectorDirections.Forward:
                    return new IntVector(X, Y, Z + 1);

                case ConnectorDirections.Backward:
                    return new IntVector(X, Y, Z - 1);
            }

            return this;
        }

        #endregion
    }
}

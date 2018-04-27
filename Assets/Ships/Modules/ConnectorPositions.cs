using System;
using Assets.Common.Utils;
using Assets.Data;

namespace Assets.Ships.Modules
{
    public struct ConnectorPosition
    {
        /// <summary>
        /// The position on the module this connector sits
        /// </summary>
        public ConnectorPositions Direction { get; set; }

        /// <summary>
        /// What different materials can this connector carry?
        /// </summary>
        public Materials[] MaterialsConveyed { get; set; }

        /// <summary>
        /// Where in the module does this connector sit?
        /// <value>{0, 0, 0} by default</value>
        /// </summary>
        public IntVector Position { get; set; }

        public ConnectorPosition(ConnectorPositions direction, Materials[] materialsConveyed)
        {
            Direction = direction;
            MaterialsConveyed = materialsConveyed;
            Position = IntVector.Zero;
        }

        public ConnectorPosition(ConnectorPositions direction, Materials[] materialsConveyed, IntVector position)
        {
            Direction = direction;
            MaterialsConveyed = materialsConveyed;
            Position = position;
        }

        public static ConnectorPositions GetOpposite(ConnectorPositions direction)
        {
            ConnectorPositions r;

            switch (direction)
            {
                case ConnectorPositions.Up:
                    r = ConnectorPositions.Down;
                    break;
                case ConnectorPositions.Right:
                    r = ConnectorPositions.Left;
                    break;
                case ConnectorPositions.Down:
                    r = ConnectorPositions.Up;
                    break;
                case ConnectorPositions.Left:
                    r = ConnectorPositions.Right;
                    break;
                case ConnectorPositions.Forward:
                    r = ConnectorPositions.Backward;
                    break;
                case ConnectorPositions.Backward:
                    r = ConnectorPositions.Forward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return r;
        }

        public static bool operator ==(ConnectorPosition a, ConnectorPosition b)
        {
            var isSame = true;

            if (a.Position != b.Position)
                isSame = false;
            else if (a.Direction != b.Direction)
                isSame = false;
            else
            {
                if (a.MaterialsConveyed == null)
                {
                    if (b.MaterialsConveyed != null)
                    {
                        if (b.MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else if (b.MaterialsConveyed == null)
                {
                    if (a.MaterialsConveyed != null)
                    {
                        if (a.MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else
                    foreach (var matA in a.MaterialsConveyed)
                    {
                        var matchFound = false;

                        foreach (var matB in b.MaterialsConveyed)
                            if (matA == matB)
                                matchFound = true;

                        if (!matchFound)
                            isSame = false;
                    }
            }

            return isSame;
        }

        public static bool operator !=(ConnectorPosition a, ConnectorPosition b)
        {
            return !(a == b);
        }

        public bool Equals(ConnectorPosition other)
        {
            var isSame = true;

            if (Position != other.Position)
                isSame = false;
            else if (Direction != other.Direction)
                isSame = false;
            else
            {
                if (MaterialsConveyed == null)
                {
                    if (other.MaterialsConveyed != null)
                    {
                        if (other.MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else if (other.MaterialsConveyed == null)
                {
                    if (MaterialsConveyed != null)
                    {
                        if (MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else
                    foreach (var matA in MaterialsConveyed)
                    {
                        var matchFound = false;

                        foreach (var matB in other.MaterialsConveyed)
                            if (matA == matB)
                                matchFound = true;

                        if (!matchFound)
                            isSame = false;
                    }
            }

            return isSame;
        }

        /// <summary>
        /// Compares this connector with the specified connector, adjusting this connector with the position supplied
        /// </summary>
        /// <param name="thisPosition">If this connector's position is relative to a module, the position of the module (if not, use ==)</param>
        /// <param name="other">The ConnectorPosition to compare against</param>
        /// <returns></returns>
        public bool Equals(IntVector thisPosition, ConnectorPosition other)
        {
            var isSame = true;

            if (Position + thisPosition != other.Position)
                isSame = false;
            else if (Direction != other.Direction)
                isSame = false;
            else
            {
                if (MaterialsConveyed == null)
                {
                    if (other.MaterialsConveyed != null)
                    {
                        if (other.MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else if (other.MaterialsConveyed == null)
                {
                    if (MaterialsConveyed != null)
                    {
                        if (MaterialsConveyed.Length > 0)
                            isSame = false;
                    }
                }
                else
                    foreach (var matA in MaterialsConveyed)
                    {
                        var matchFound = false;

                        foreach (var matB in other.MaterialsConveyed)
                            if (matA == matB)
                                matchFound = true;

                        if (!matchFound)
                            isSame = false;
                    }
            }

            return isSame;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ConnectorPosition && Equals((ConnectorPosition)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int)Direction;
                hashCode = (hashCode * 397) ^ (MaterialsConveyed != null ? MaterialsConveyed.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                return hashCode;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Position, Direction);
        }
    }

    public enum ConnectorPositions
    {
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4,
        Forward = 5,
        Backward = 6
    }
}

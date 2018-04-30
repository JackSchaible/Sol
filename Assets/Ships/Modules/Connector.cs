using System;
using Assets.Data;
using UnityEngine;

namespace Assets.Ships.Modules
{
    public struct Connector
    {
        /// <summary>
        /// The position on the module this connector sits
        /// </summary>
        public ConnectorDirections Direction { get; set; }

        /// <summary>
        /// What different materials can this connector carry?
        /// </summary>
        public Materials[] MaterialsConveyed { get; set; }

        /// <summary>
        /// Where in the module does this connector sit?
        /// <value>{0, 0, 0} by default</value>
        /// </summary>
        public Vector3Int Position { get; set; }

        public Connector(ConnectorDirections direction, Materials[] materialsConveyed)
        {
            Direction = direction;
            MaterialsConveyed = materialsConveyed;
            Position = Vector3Int.zero;
        }

        public Connector(ConnectorDirections direction, Materials[] materialsConveyed, Vector3Int position)
        {
            Direction = direction;
            MaterialsConveyed = materialsConveyed;
            Position = position;
        }

        public static ConnectorDirections GetOpposite(ConnectorDirections direction)
        {
            ConnectorDirections r;

            switch (direction)
            {
                case ConnectorDirections.Up:
                    r = ConnectorDirections.Down;
                    break;
                case ConnectorDirections.Right:
                    r = ConnectorDirections.Left;
                    break;
                case ConnectorDirections.Down:
                    r = ConnectorDirections.Up;
                    break;
                case ConnectorDirections.Left:
                    r = ConnectorDirections.Right;
                    break;
                case ConnectorDirections.Forward:
                    r = ConnectorDirections.Backward;
                    break;
                case ConnectorDirections.Backward:
                    r = ConnectorDirections.Forward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return r;
        }

        public static bool operator ==(Connector a, Connector b)
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

        public static bool operator !=(Connector a, Connector b)
        {
            return !(a == b);
        }

        public bool Equals(Connector other)
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Connector && Equals((Connector)obj);
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

    public enum ConnectorDirections
    {
        Up = 1,
        Right = 2,
        Down = 3,
        Left = 4,
        Forward = 5,
        Backward = 6
    }
}

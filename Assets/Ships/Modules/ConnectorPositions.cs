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

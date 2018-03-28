using Assets.Common.Utils;

namespace Assets.Ships.Modules
{
    public class ConnectorPosition
    {
        /// <summary>
        /// The position on the module this connector sits
        /// </summary>
        public ConnectorPositions Direction { get; set; }

        /// <summary>
        /// Where in the module does this connector sit?
        /// <value>{0, 0, 0} by default</value>
        /// </summary>
        public IntVector Position { get; set; }

        public ConnectorPosition()
        {
            
        }

        public ConnectorPosition(ConnectorPositions direction, IntVector position = null)
        {
            Direction = direction;

            Position = position ?? IntVector.Zero;
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

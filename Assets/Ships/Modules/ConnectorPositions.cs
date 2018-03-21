namespace Assets.Ships.Modules
{
    public class ConnectorPosition
    {
        /// <summary>
        /// The position on the module this connector sits
        /// </summary>
        public ConnectorPositions Position { get; set; }

        /// <summary>
        /// Must this connector be attached to another module?
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// If the module is mandatory, does it belong to a group of "At Least 1" connectors?
        /// </summary>
        public int? Group { get; set; }

        public ConnectorPosition()
        {
            
        }

        public ConnectorPosition(ConnectorPositions position, bool isMandatory, int? @group)
        {
            Position = position;
            IsMandatory = isMandatory;
            Group = @group;
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

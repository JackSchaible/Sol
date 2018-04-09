using System;
using Assets.Ships.Modules;

namespace Assets.Utils.ModuleUtils
{
    public static class ModuleVectorUtils
    {
        public enum RotationDirection
        {
            CW,
            CCW
        }

        public enum FlipDirection
        {
            Horizontal,
            Vertical
        }

        public static ConnectorPosition[] RotateConnectorPositions(ConnectorPosition[] positions, RotationDirection rd)
        {
            foreach (ConnectorPosition cp in positions)
            {
                switch (cp.Direction)
                {
                    case ConnectorPositions.Forward:
                    case ConnectorPositions.Backward:
                        break;

                    case ConnectorPositions.Up:
                        cp.Direction = rd == RotationDirection.CW ? ConnectorPositions.Right : ConnectorPositions.Left;
                        break;

                    case ConnectorPositions.Right:
                        cp.Direction = rd == RotationDirection.CW ? ConnectorPositions.Down : ConnectorPositions.Up;
                        break;

                    case ConnectorPositions.Down:
                        cp.Direction = rd == RotationDirection.CW ? ConnectorPositions.Left : ConnectorPositions.Right;
                        break;

                    case ConnectorPositions.Left:
                        cp.Direction = rd == RotationDirection.CW ? ConnectorPositions.Up : ConnectorPositions.Down;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return positions;
        }

        public static ConnectorPosition[] FlipConnectorPositions(ConnectorPosition[] positions, FlipDirection fd)
        {
            foreach (ConnectorPosition cp in positions)
            {
                switch (cp.Direction)
                {
                    case ConnectorPositions.Forward:
                    case ConnectorPositions.Backward:
                        break;

                    case ConnectorPositions.Up:
                        if (fd == FlipDirection.Horizontal)
                            cp.Direction = ConnectorPositions.Down;
                        break;

                    case ConnectorPositions.Right:
                        if (fd == FlipDirection.Vertical)
                            cp.Direction = ConnectorPositions.Left;
                        break;

                    case ConnectorPositions.Down:
                        if (fd == FlipDirection.Horizontal)
                            cp.Direction = ConnectorPositions.Up;
                        break;

                    case ConnectorPositions.Left:
                        if (fd == FlipDirection.Vertical)
                            cp.Direction = ConnectorPositions.Right;
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return positions;
        }
    }
}

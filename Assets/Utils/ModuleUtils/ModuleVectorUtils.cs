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

        public static ExclusionVectors[] RotateExclusionVectors(ExclusionVectors[] vectors, RotationDirection rd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                switch (vectors[i])
                {
                    case ExclusionVectors.ForwardLine:
                    case ExclusionVectors.BackwardLine:
                    case ExclusionVectors.Plane:
                    case ExclusionVectors.PlaneAndForward:
                    case ExclusionVectors.PlaneAndBackward:
                        break;

                    case ExclusionVectors.UpwardLine:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.RightLine;
                        else
                            vectors[i] = ExclusionVectors.LeftLine;
                        break;

                    case ExclusionVectors.DownwardLine:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.LeftLine;
                        else
                            vectors[i] = ExclusionVectors.RightLine;
                        break;

                    case ExclusionVectors.RightLine:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.DownwardLine;
                        else
                            vectors[i] = ExclusionVectors.UpwardLine;
                        break;

                    case ExclusionVectors.LeftLine:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.UpwardLine;
                        else
                            vectors[i] = ExclusionVectors.DownwardLine;
                        break;

                    case ExclusionVectors.PlaneAndAbove:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.PlaneAndRight;
                        else
                            vectors[i] = ExclusionVectors.PlaneAndLeft;
                        break;

                    case ExclusionVectors.PlaneAndBelow:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.PlaneAndLeft;
                        else
                            vectors[i] = ExclusionVectors.PlaneAndRight;
                        break;


                    case ExclusionVectors.PlaneAndRight:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.PlaneAndBelow;
                        else
                            vectors[i] = ExclusionVectors.PlaneAndAbove;
                        break;

                    case ExclusionVectors.PlaneAndLeft:
                        if (rd == RotationDirection.CW)
                            vectors[i] = ExclusionVectors.PlaneAndAbove;
                        else
                            vectors[i] = ExclusionVectors.PlaneAndBelow;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return vectors;
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

        public static ExclusionVectors[] FlipExclusionVectors(ExclusionVectors[] vectors, FlipDirection fd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                switch (vectors[i])
                {
                    case ExclusionVectors.ForwardLine:
                    case ExclusionVectors.BackwardLine:
                    case ExclusionVectors.Plane:
                    case ExclusionVectors.PlaneAndForward:
                    case ExclusionVectors.PlaneAndBackward:
                        break;

                    case ExclusionVectors.UpwardLine:
                        if (fd == FlipDirection.Horizontal)
                            vectors[i] = ExclusionVectors.DownwardLine;
                        break;

                    case ExclusionVectors.DownwardLine:
                        if (fd == FlipDirection.Horizontal)
                            vectors[i] = ExclusionVectors.UpwardLine;
                        break;

                    case ExclusionVectors.RightLine:
                        if (fd == FlipDirection.Vertical)
                            vectors[i] = ExclusionVectors.LeftLine;
                        break;

                    case ExclusionVectors.LeftLine:
                        if (fd == FlipDirection.Vertical)
                            vectors[i] = ExclusionVectors.RightLine;
                        break;

                    case ExclusionVectors.PlaneAndAbove:
                        if (fd == FlipDirection.Horizontal)
                            vectors[i] = ExclusionVectors.PlaneAndBelow;
                        break;

                    case ExclusionVectors.PlaneAndBelow:
                        if (fd == FlipDirection.Horizontal)
                            vectors[i] = ExclusionVectors.PlaneAndAbove;
                        break;


                    case ExclusionVectors.PlaneAndRight:
                        if (fd == FlipDirection.Vertical)
                            vectors[i] = ExclusionVectors.PlaneAndLeft;
                        break;

                    case ExclusionVectors.PlaneAndLeft:
                        if (fd == FlipDirection.Vertical)
                            vectors[i] = ExclusionVectors.PlaneAndRight;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return vectors;
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

using System;
using Assets.Common.Utils;
using Assets.Ships.Modules;
using UnityEngine;

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

        public static Connector[] RotateConnectorPositions(Connector[] positions, RotationDirection rd)
        {
            for (var i = 0; i < positions.Length; i++)
            {
                var pos = new Vector3Int(positions[i].Position.y, -positions[i].Position.x, positions[i].Position.z);
                ConnectorDirections direction = ConnectorDirections.Forward;

                switch (positions[i].Direction)
                {
                    case ConnectorDirections.Forward:
                        direction = ConnectorDirections.Forward;
                        break;

                    case ConnectorDirections.Backward:
                        direction = ConnectorDirections.Backward;
                        break;

                    case ConnectorDirections.Up:
                        direction = rd == RotationDirection.CW ? ConnectorDirections.Right : ConnectorDirections.Left;
                        break;

                    case ConnectorDirections.Right:
                        direction = rd == RotationDirection.CW ? ConnectorDirections.Down : ConnectorDirections.Up;
                        break;

                    case ConnectorDirections.Down:
                        direction = rd == RotationDirection.CW ? ConnectorDirections.Left : ConnectorDirections.Right;
                        break;

                    case ConnectorDirections.Left:
                        direction = rd == RotationDirection.CW ? ConnectorDirections.Up : ConnectorDirections.Down;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                positions[i] = new Connector(direction, positions[i].MaterialsConveyed, pos);
            }

            return positions;
        }

        public static Connector[] FlipConnectorPositions(Connector[] positions, FlipDirection fd)
        {
            foreach (Connector cp in positions)
            {
                //switch (cp.Direction)
                //{
                //    case ConnectorDirections.Forward:
                //    case ConnectorDirections.Backward:
                //        break;

                //    case ConnectorDirections.Up:
                //        if (fd == FlipDirection.Horizontal)
                //            cp.Direction = ConnectorDirections.Down;
                //        break;

                //    case ConnectorDirections.Right:
                //        if (fd == FlipDirection.Vertical)
                //            cp.Direction = ConnectorDirections.Left;
                //        break;

                //    case ConnectorDirections.Down:
                //        if (fd == FlipDirection.Horizontal)
                //            cp.Direction = ConnectorDirections.Up;
                //        break;

                //    case ConnectorDirections.Left:
                //        if (fd == FlipDirection.Vertical)
                //            cp.Direction = ConnectorDirections.Right;
                //        break;
                    
                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
            }

            return positions;
        }

        public static ExclusionVector[] RotateExclusionVectors(ExclusionVector[] vectors, RotationDirection rd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                //switch (vectors[i])
                //{
                //    case ExclusionVectors.ForwardLine:
                //    case ExclusionVectors.BackwardLine:
                //    case ExclusionVectors.Plane:
                //    case ExclusionVectors.PlaneAndForward:
                //    case ExclusionVectors.PlaneAndBackward:
                //        break;

                //    case ExclusionVectors.UpwardLine:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.RightLine;
                //        else
                //            vectors[i] = ExclusionVectors.LeftLine;
                //        break;

                //    case ExclusionVectors.DownwardLine:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.LeftLine;
                //        else
                //            vectors[i] = ExclusionVectors.RightLine;
                //        break;

                //    case ExclusionVectors.RightLine:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.DownwardLine;
                //        else
                //            vectors[i] = ExclusionVectors.UpwardLine;
                //        break;

                //    case ExclusionVectors.LeftLine:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.UpwardLine;
                //        else
                //            vectors[i] = ExclusionVectors.DownwardLine;
                //        break;

                //    case ExclusionVectors.PlaneAndAbove:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.PlaneAndRight;
                //        else
                //            vectors[i] = ExclusionVectors.PlaneAndLeft;
                //        break;

                //    case ExclusionVectors.PlaneAndBelow:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.PlaneAndLeft;
                //        else
                //            vectors[i] = ExclusionVectors.PlaneAndRight;
                //        break;


                //    case ExclusionVectors.PlaneAndRight:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.PlaneAndBelow;
                //        else
                //            vectors[i] = ExclusionVectors.PlaneAndAbove;
                //        break;

                //    case ExclusionVectors.PlaneAndLeft:
                //        if (rd == RotationDirection.CW)
                //            vectors[i] = ExclusionVectors.PlaneAndAbove;
                //        else
                //            vectors[i] = ExclusionVectors.PlaneAndBelow;
                //        break;

                //    default:
                //        throw new ArgumentOutOfRangeException();
                //}
            }

            return vectors;
        }

        public static ExclusionVector[] FlipExclusionVectors(ExclusionVector[] vectors, FlipDirection fd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                for (var j = 0; j < vectors[i].Direction.Length; j++)
                    switch (vectors[i].Direction[j])
                    {
                    //    case ExclusionVectors.ForwardLine:
                    //    case ExclusionVectors.BackwardLine:
                    //    case ExclusionVectors.Plane:
                    //    case ExclusionVectors.PlaneAndForward:
                    //    case ExclusionVectors.PlaneAndBackward:
                    //        break;

                    //    case ExclusionVectors.UpwardLine:
                    //        if (fd == FlipDirection.Horizontal)
                    //            vectors[i] = ExclusionVectors.DownwardLine;
                    //        break;

                    //    case ExclusionVectors.DownwardLine:
                    //        if (fd == FlipDirection.Horizontal)
                    //            vectors[i] = ExclusionVectors.UpwardLine;
                    //        break;

                    //    case ExclusionVectors.RightLine:
                    //        if (fd == FlipDirection.Vertical)
                    //            vectors[i] = ExclusionVectors.LeftLine;
                    //        break;

                    //    case ExclusionVectors.LeftLine:
                    //        if (fd == FlipDirection.Vertical)
                    //            vectors[i] = ExclusionVectors.RightLine;
                    //        break;

                    //    case ExclusionVectors.PlaneAndAbove:
                    //        if (fd == FlipDirection.Horizontal)
                    //            vectors[i] = ExclusionVectors.PlaneAndBelow;
                    //        break;

                    //    case ExclusionVectors.PlaneAndBelow:
                    //        if (fd == FlipDirection.Horizontal)
                    //            vectors[i] = ExclusionVectors.PlaneAndAbove;
                    //        break;


                    //    case ExclusionVectors.PlaneAndRight:
                    //        if (fd == FlipDirection.Vertical)
                    //            vectors[i] = ExclusionVectors.PlaneAndLeft;
                    //        break;

                    //    case ExclusionVectors.PlaneAndLeft:
                    //        if (fd == FlipDirection.Vertical)
                    //            vectors[i] = ExclusionVectors.PlaneAndRight;
                    //        break;

                    //    default:
                    //        throw new ArgumentOutOfRangeException();
                    }
            }

            return vectors;
        }

        public static IntVector[] RotateSpace(IntVector[] space, RotationDirection rd)
        {
            if (rd == RotationDirection.CW)
                for (int i = 0; i < space.Length; i++)
                    space[i] = new IntVector(space[i].Y, -space[i].X, space[i].Z);
            else
                for (int i = 0; i < space.Length; i++)
                    space[i] = new IntVector(-space[i].Y, space[i].X, space[i].Z);
            
            return space;
        }

        public static IntVector[] FlipSpace(IntVector[] space, FlipDirection fd)
        {
            if (fd == FlipDirection.Horizontal)
                for(int i = 0; i < space.Length; i++)
                    space[i] = new IntVector(-space[i].X, space[i].Y, space[i].Z);
            else
                for (int i = 0; i < space.Length; i++)
                    space[i] = new IntVector(space[i].X, -space[i].Y, space[i].Z);

            return space;
        }
    }
}

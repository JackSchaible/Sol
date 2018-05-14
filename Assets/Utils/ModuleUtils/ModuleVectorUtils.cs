using System.Collections.Generic;
using Assets.Ships;
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

        public static Module RotateModule(Module mod, RotationDirection rd)
        {
            for (int i = 0; i < mod.Components.Length; i++)
            {
                var newLocalPosition = Rotate(mod.Components[i].LocalPosition, rd);
                var newGameObject = mod.Components[i].GameObject;
                newGameObject.transform.Rotate(Vector3.forward, rd == RotationDirection.CW ? -90f : 90f);
                var newConnectors = RotateConnectorPositions(mod.Components[i].Connectors, rd);
                var newExclusionVectors = RotateExclusionVectorDirections(mod.Components[i].ExclusionVectors, rd);
                var newCom = new ModuleComponent(newGameObject, newLocalPosition, newConnectors, newExclusionVectors);

                mod.Components[i] = newCom;
            }

            mod.ModuleBlueprint.Space = RotateSpace(mod.ModuleBlueprint.Space, rd);

            return mod;
        }
        public static Vector3Int Rotate(Vector3Int a, RotationDirection rd)
        {
            return rd == RotationDirection.CW ?
                new Vector3Int(a.y, -a.x, a.z) :
                new Vector3Int(-a.y, a.x, a.z);
        }
        public static Vector3Int Rotate(Vector3Int a, int rotations)
        {
            for (int i = 0; i < Mathf.Abs(rotations); i++)
                a = Rotate(a,
                    rotations > 0
                        ? RotationDirection.CCW
                        : RotationDirection.CW);

            return a;
        }

        private static Connector[] RotateConnectorPositions(Connector[] positions, RotationDirection rd)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var pos = Rotate(positions[i].Position, rd);
                var dir = Rotate(positions[i].Direction, rd);

                positions[i] = new Connector(dir, positions[i].MaterialsConveyed, pos);
            }

            return positions;
        }
        private static ExclusionVector[] RotateExclusionVectorDirections(ExclusionVector[] vectors, RotationDirection rd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                var newDirections = new ExclusionVectorDirections[vectors[i].Direction.Length];

                for (int j = 0; j < vectors[i].Direction.Length; j++)
                {
                    ExclusionVectorDirections newDirection = ExclusionVectorDirections.BackwardLine;

                    switch (vectors[i].Direction[j])
                    {
                        case ExclusionVectorDirections.ForwardLine:
                        case ExclusionVectorDirections.BackwardLine:
                        case ExclusionVectorDirections.Plane:
                        case ExclusionVectorDirections.PlaneAndForward:
                        case ExclusionVectorDirections.PlaneAndBackward:
                            break;

                        case ExclusionVectorDirections.UpwardLine:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.RightLine;
                            else
                                newDirection = ExclusionVectorDirections.LeftLine;
                            break;

                        case ExclusionVectorDirections.DownwardLine:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.LeftLine;
                            else
                                newDirection = ExclusionVectorDirections.RightLine;
                            break;

                        case ExclusionVectorDirections.RightLine:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.DownwardLine;
                            else
                                newDirection = ExclusionVectorDirections.UpwardLine;
                            break;

                        case ExclusionVectorDirections.LeftLine:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.UpwardLine;
                            else
                                newDirection = ExclusionVectorDirections.DownwardLine;
                            break;

                        case ExclusionVectorDirections.PlaneAndAbove:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.PlaneAndRight;
                            else
                                newDirection = ExclusionVectorDirections.PlaneAndLeft;
                            break;

                        case ExclusionVectorDirections.PlaneAndBelow:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.PlaneAndLeft;
                            else
                                newDirection = ExclusionVectorDirections.PlaneAndRight;
                            break;


                        case ExclusionVectorDirections.PlaneAndRight:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.PlaneAndBelow;
                            else
                                newDirection = ExclusionVectorDirections.PlaneAndAbove;
                            break;

                        case ExclusionVectorDirections.PlaneAndLeft:
                            if (rd == RotationDirection.CW)
                                newDirection = ExclusionVectorDirections.PlaneAndAbove;
                            else
                                newDirection = ExclusionVectorDirections.PlaneAndBelow;
                            break;
                    }

                    newDirections[j] = newDirection;
                }

                var newPosition = Rotate(vectors[i].Position, rd);

                vectors[i] = new ExclusionVector(newDirections, newPosition);
            }

            return vectors;
        }
        private static Vector3Int[] RotateSpace(Vector3Int[] space, RotationDirection rd)
        {
            for (int i = 0; i < space.Length; i++)
                space[i] = Rotate(space[i], rd);

            return space;
        }

        //TODO: Flip
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

        //public static IntVector[] FlipSpace(IntVector[] space, FlipDirection fd)
        //{
        //    if (fd == FlipDirection.Horizontal)
        //        for (int i = 0; i < space.Length; i++)
        //            space[i] = new IntVector(-space[i].X, space[i].Y, space[i].Z);
        //    else
        //        for (int i = 0; i < space.Length; i++)
        //            space[i] = new IntVector(space[i].X, -space[i].Y, space[i].Z);

        //    return space;
        //}
    }
}

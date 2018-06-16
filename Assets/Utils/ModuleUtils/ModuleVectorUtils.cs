using System;
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
                var newCom = new ModuleComponent(newGameObject, mod.Components[i].BuildSprite, mod.Components[i].Sprite, newLocalPosition, newConnectors, newExclusionVectors);

                mod.Components[i] = newCom;
            }

            mod.ModuleBlueprint.Space = RotateSpace(mod.ModuleBlueprint.Space, rd);

            return mod;
        }
        public static Module RotateModule(Module mod, int rotations)
        {
            for (int i = 0; i < rotations; i++)
                mod = RotateModule(mod,
                    rotations > 0 ? RotationDirection.CW : RotationDirection.CCW);

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

        public static Module FlipModule(Module mod, FlipDirection fd)
        {
            for (int i = 0; i < mod.Components.Length; i++)
            {
                var newLocalPosition = Flip(mod.Components[i].LocalPosition, fd);
                var newGameObject = mod.Components[i].GameObject;
                var spriteRenderer = newGameObject.GetComponent<SpriteRenderer>();
                switch (fd)
                {
                    case FlipDirection.Horizontal:
                        spriteRenderer.flipX = !spriteRenderer.flipX;
                        break;

                    case FlipDirection.Vertical:
                        spriteRenderer.flipY = !spriteRenderer.flipY;
                        break;
                }
                var newConnectors = FlipConnectorPositions(mod.Components[i].Connectors, fd);
                var newExclusionVectors = FlipExclusionVectorDirections(mod.Components[i].ExclusionVectors, fd);
                var newCom = new ModuleComponent(newGameObject, mod.Components[i].BuildSprite, mod.Components[i].Sprite, newLocalPosition, newConnectors, newExclusionVectors);

                mod.Components[i] = newCom;
            }

            mod.ModuleBlueprint.Space = FlipSpace(mod.ModuleBlueprint.Space, fd);

            return mod;

        }
        public static Module FlipModule(Module mod, int[] flips)
        {
            if (flips[0] == 1)
                mod = FlipModule(mod, FlipDirection.Horizontal);

            if (flips[1] == 1)
                mod = FlipModule(mod, FlipDirection.Vertical);

            return mod;
        }
        public static Vector3Int Flip(Vector3Int a, FlipDirection fd)
        {
            switch (fd)
            {
                case FlipDirection.Horizontal:
                    a = new Vector3Int(-a.x, a.y, a.z);
                    break;
                case FlipDirection.Vertical:
                    a = new Vector3Int(a.x, -a.y, a.z);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fd", fd, null);
            }

            return a;
        }

        public static Connector[] RotateConnectorPositions(Connector[] connectors, int rotations)
        {
            for (int i = 0; i < rotations; i++)
                connectors = RotateConnectorPositions(connectors,
                    rotations > 0 ? RotationDirection.CW : RotationDirection.CCW);

            return connectors;
        }
        public static Connector[] FlipConnectorPositions(Connector[] connectors, int[] flips)
        {
            if (flips[0] == 1)
                connectors = FlipConnectorPositions(connectors, FlipDirection.Horizontal);

            if (flips[1] == 1)
                connectors = FlipConnectorPositions(connectors, FlipDirection.Vertical);

            return connectors;
        }

        public static Vector3Int Multiply(Vector3Int vector, int scalar)
        {
            return new Vector3Int(vector.x * scalar, vector.y * scalar, vector.z * scalar);
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
                var newDirections = new ExclusionVectorDirections[vectors[i].Directions.Length];

                for (int j = 0; j < vectors[i].Directions.Length; j++)
                {
                    ExclusionVectorDirections newDirection = ExclusionVectorDirections.BackwardLine;

                    switch (vectors[i].Directions[j])
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

        private static Connector[] FlipConnectorPositions(Connector[] positions, FlipDirection fd)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                var pos = Flip(positions[i].Position, fd);
                var dir = Flip(positions[i].Direction, fd);

                positions[i] = new Connector(dir, positions[i].MaterialsConveyed, pos);
            }

            return positions;
        }
        private static ExclusionVector[] FlipExclusionVectorDirections(ExclusionVector[] vectors, FlipDirection fd)
        {
            for (var i = 0; i < vectors.Length; i++)
            {
                var newDirections = new ExclusionVectorDirections[vectors[i].Directions.Length];

                for (int j = 0; j < vectors[i].Directions.Length; j++)
                {
                    ExclusionVectorDirections newDirection = vectors[i].Directions[j];

                    if (fd == FlipDirection.Vertical)
                    {
                        switch (vectors[i].Directions[j])
                        {
                            case ExclusionVectorDirections.UpwardLine:
                                newDirection = ExclusionVectorDirections.DownwardLine;
                                break;
                            case ExclusionVectorDirections.DownwardLine:
                                newDirection = ExclusionVectorDirections.UpwardLine;
                                break;
                            case ExclusionVectorDirections.PlaneAndAbove:
                                newDirection = ExclusionVectorDirections.PlaneAndBelow;
                                break;
                            case ExclusionVectorDirections.PlaneAndBelow:
                                newDirection = ExclusionVectorDirections.PlaneAndAbove;
                                break;
                        }
                    }
                    else
                    {
                        switch (vectors[i].Directions[j])
                        {
                            case ExclusionVectorDirections.RightLine:
                                newDirection = ExclusionVectorDirections.LeftLine;
                                break;
                            case ExclusionVectorDirections.LeftLine:
                                newDirection = ExclusionVectorDirections.RightLine;
                                break;
                            case ExclusionVectorDirections.PlaneAndRight:
                                newDirection = ExclusionVectorDirections.PlaneAndLeft;
                                break;
                            case ExclusionVectorDirections.PlaneAndLeft:
                                newDirection = ExclusionVectorDirections.PlaneAndRight;
                                break;
                        }
                    }

                    newDirections[j] = newDirection;
                }

                var newPosition = Flip(vectors[i].Position, fd);

                vectors[i] = new ExclusionVector(newDirections, newPosition);
            }

            return vectors;
        }
        private static Vector3Int[] FlipSpace(Vector3Int[] space, FlipDirection fd)
        {
            for (int i = 0; i < space.Length; i++)
                space[i] = Flip(space[i], fd);

            return space;
        }
    }
}

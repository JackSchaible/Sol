using UnityEngine;

namespace Assets.Ships.Modules
{
    public struct ExclusionVector
    {
        public ExclusionVectorDirections[] Directions { get; set; }
        public Vector3Int Position { get; set; }

        public ExclusionVector(ExclusionVectorDirections[] directions)
        {
            Directions = directions;
            Position = Vector3Int.zero;
        }

        public ExclusionVector(ExclusionVectorDirections[] directions, Vector3Int position)
        {
            Directions = directions;
            Position = position;
        }
    }

    public enum ExclusionVectorDirections
    {
        ForwardLine,
        BackwardLine,
        UpwardLine,
        DownwardLine,
        RightLine,
        LeftLine,
        Plane,
        PlaneAndAbove,
        PlaneAndBelow,
        PlaneAndForward,
        PlaneAndBackward,
        PlaneAndRight,
        PlaneAndLeft
    }
}

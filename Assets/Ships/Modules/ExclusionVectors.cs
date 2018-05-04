using UnityEngine;

namespace Assets.Ships.Modules
{
    public struct ExclusionVector
    {
        public ExclusionVectorDirections[] Direction { get; set; }
        public Vector3Int Position { get; set; }

        public ExclusionVector(ExclusionVectorDirections[] direction)
        {
            Direction = direction;
            Position = Vector3Int.zero;
        }

        public ExclusionVector(ExclusionVectorDirections[] direction, Vector3Int position)
        {
            Direction = direction;
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

using Assets.Common.Utils;

namespace Assets.Ships.Modules
{
    public class ExclusionVector
    {
        public ExclusionVectorDirections[] Direction { get; set; }
        public IntVector Position { get; set; }

        public ExclusionVector(ExclusionVectorDirections[] direction)
        {
            Direction = direction;
            Position = IntVector.Zero;
        }

        public ExclusionVector(ExclusionVectorDirections[] direction, IntVector position)
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

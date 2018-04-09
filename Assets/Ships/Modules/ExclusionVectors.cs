using Assets.Common.Utils;

namespace Assets.Ships.Modules
{
    public class ExclusionVector
    {
        public ExclusionVectorDirections[] Direction { get; set; }
        public IntVector Position { get; set; }

        public ExclusionVector(ExclusionVectorDirections[] direction, IntVector position = null)
        {
            Direction = direction;

            if (position == null)
                position = IntVector.Zero;
            else
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

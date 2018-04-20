using System;

namespace Assets.Utils
{
    static class MathHelper
    {
        #region Clamp
        public static int Clamp(int value, int min, int max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value > max)
                return max;

            if (value < min)
                return min;

            return value;
        }
        #endregion

        #region Clamp Angle
        public static float ClampAngleDegrees(float angle)
        {
            if (angle < 0)
                while (angle < 0)
                    angle = 360 - angle;
            else if (angle > 360)
                while (angle > 360)
                    angle = angle - 360;

            return angle;
        }

        public static float ClampAngleRadians(float angle)
        {
            if (angle < 0)
                while (angle < 0)
                    angle = (float)Math.PI - angle;
            else if (angle > 360)
                while (angle > 360)
                    angle = angle - (float)Math.PI;

            return angle;
        }
        #endregion
    }
}

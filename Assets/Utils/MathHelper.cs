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

        #region AjustSI

        public static string AdjustSI(float unadjustedValue, int decimalPlaces)
        {
			//TODO: broken
            var value = unadjustedValue;
            float factor = unadjustedValue < 0 ? 0.001f : 1000f;
            int iterations = 0;
            string prefix = "";

            while (value.ToString().Length > decimalPlaces)
            {
                value *= factor;

                iterations++;
            }

            if (iterations > 0)
            {
                if (unadjustedValue > 0)
                    switch (iterations)
                    {
                        case 1:
                            prefix = "k";
                            break;

                        case 2:
                            prefix = "M";
                            break;
                        case 3:
                            prefix = "G";
                            break;
                        case 4:
                            prefix = "T";
                            break;
                        case 5:
                            prefix = "P";
                            break;
                        case 6:
                            prefix = "E";
                            break;
                        case 7:
                            prefix = "Z";
                            break;
                        case 8:
                            prefix = "Y";
                            break;
                    }
                else
                    switch (iterations)
                    {
                        case 1:
                            prefix = "m";
                            break;
                        case 2:
                            prefix = "µ";
                            break;
                        case 3:
                            prefix = "n";
                            break;
                        case 4:
                            prefix = "p";
                            break;
                        case 5:
                            prefix = "f";
                            break;
                        case 6:
                            prefix = "a";
                            break;
                        case 7:
                            prefix = "z";
                            break;
                        case 8:
                            prefix = "y"; break;
                    }
            }

            return String.Format("{0} {1}", value, prefix);
        }
        #endregion
    }
}

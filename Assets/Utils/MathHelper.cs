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

        public static string AdjustSi(float number, string baseUnit)
        {
            string result = "";
            bool isNegative = number < 0;

            if (isNegative)
                number *= -1;

            if (number >= 10E23)
                result = Math.Round(number / 10E23) + "Y" + baseUnit;
            else if (number >= 10E20)
                result = Math.Round(number / 10E20) + "Z" + baseUnit;
            else if (number >= 10E17)
                result = Math.Round(number / 10E17) + "E" + baseUnit;
            else if (number >= 10E14)
                result = Math.Round(number / 10E14) + "P" + baseUnit;
            else if (number >= 10E11)
                result = Math.Round(number / 10E11) + "T" + baseUnit;
            else if (number >= 10E8)
                result = Math.Round(number / 10E8) + "G" + baseUnit;
            else if (number >= 10E5)
                result = Math.Round(number / 10E5) + "M" + baseUnit;
            else if (number >= 10E2)
                result = Math.Round(number / 10E2) + "k" + baseUnit;
            else
                result = number + baseUnit;

            if (isNegative)
                result = "-" + result;

            return result;
        }

        #endregion
    }
}

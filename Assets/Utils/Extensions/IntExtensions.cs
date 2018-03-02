using System;

namespace Assets.Utils.Extensions
{
    public static class IntExtensions
    {
        public static string ToRomanNumeral(this int number)
        {
            if ((number < 0) || (number > 3999)) throw new ArgumentOutOfRangeException("insert value betwheen 1 and 3999");
            if (number < 1) return string.Empty;
            if (number >= 1000) return "M" + ToRomanNumeral(number - 1000);
            if (number >= 900) return "CM" + ToRomanNumeral(number - 900);
            if (number >= 500) return "D" + ToRomanNumeral(number - 500);
            if (number >= 400) return "CD" + ToRomanNumeral(number - 400);
            if (number >= 100) return "C" + ToRomanNumeral(number - 100);
            if (number >= 90) return "XC" + ToRomanNumeral(number - 90);
            if (number >= 50) return "L" + ToRomanNumeral(number - 50);
            if (number >= 40) return "XL" + ToRomanNumeral(number - 40);
            if (number >= 10) return "X" + ToRomanNumeral(number - 10);
            if (number >= 9) return "IX" + ToRomanNumeral(number - 9);
            if (number >= 5) return "V" + ToRomanNumeral(number - 5);
            if (number >= 4) return "IV" + ToRomanNumeral(number - 4);
            if (number >= 1) return "I" + ToRomanNumeral(number - 1);
            throw new ArgumentOutOfRangeException("something bad happened");
        }

        public static string ToSiUnit(this int number, string unit)
        {
            string result = "";
            bool isNegative = number < 0;

            if (isNegative)
                number *= -1;

            if (number >= 10E23)
                result = Math.Round(number / 10E23) + "Y" + unit;
            else if (number >= 10E20)
                result = Math.Round(number / 10E20) + "Z" + unit;
            else if (number >= 10E17)
                result = Math.Round(number / 10E17) + "E" + unit;
            else if (number >= 10E14)
                result = Math.Round(number / 10E14) + "P" + unit;
            else if (number >= 10E11)
                result = Math.Round(number / 10E11) + "T" + unit;
            else if (number >= 10E8)
                result = Math.Round(number / 10E8) + "G" + unit;
            else if (number >= 10E5)
                result = Math.Round(number / 10E5) + "M" + unit;
            else if (number >= 10E2)
                result = Math.Round(number / 10E2) + "k" + unit;
            else
                result = number + unit;

            if (isNegative)
                result = "-" + result;

            return result;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Assets.Utils.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// This method parses the string until it reaches a character it can't parse
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <returns>An integer representing the string</returns>
        public static int ParseUntil(this string s)
        {
            int n, i = 0, rVal = 0;
            var numbers = new List<int>();
            bool hasEncounteredNegative = false;

            if (s[i] == '-')
            {
                i++;
                hasEncounteredNegative = true;
            }

            while (int.TryParse(s[i].ToString(), out n))
            {
                i++;
                numbers.Add(n);

                if (i > s.Length - 1) break;
                if (s[i] == '-' && numbers.Count == 0) hasEncounteredNegative = true;
            }

            numbers.Reverse();

            for (i = numbers.Count - 1; i >= 0; i--)
                rVal += numbers[i] * (int)Math.Pow(10, i);

            if (hasEncounteredNegative)
                rVal *= -1;

            return rVal;
        }

        public static List<int> IndicesOf(this string str, string value)
        {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", "value");
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    return indexes;
                indexes.Add(index);
            }
        }
    }
}

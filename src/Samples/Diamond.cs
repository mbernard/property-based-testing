using System;
using System.Collections.Generic;
using System.Text;

namespace Samples
{
    public class Diamond
    {
        public static IEnumerable<string> Generate(char c)
        {
            return GenerateFinal(c);
        }

        private static IEnumerable<string> Generate1(char c)
        {
            yield return "A";
        }

        private static IEnumerable<string> Generate2(char c)
        {
            yield return " A ";
            yield return $"{c} {c}";
            yield return " A ";
        }

        private static IEnumerable<string> Generate3(char c)
        {
            if (c == 'A')
            {
                yield return "A";
            }
            else
            {
                yield return " A ";
                yield return $"{c} {c}";
                yield return " A ";
            }
        }

        private static IEnumerable<string> Generate4(char c)
        {
            var length = (c - 'A' + 1) * 2 - 1;
            var edge = "{0}" + new string(' ', Math.Max(length - 2, 0)) + "{0}";
            var lastIndex = length - 1;
            var aPadding = new string(' ', (length - 1) / 2);

            for (int i = 0; i < length; i++)
            {
                if (i == 0 || i == lastIndex)
                {
                    yield return  aPadding + 'A' + aPadding;
                }
                else
                {
                    char x = (char)((i <= length / 2) ? ('A' + i) : (c - (i - length/2)));
                    yield return string.Format(edge, x);
                }
            }
        }

        private static IEnumerable<string> GenerateFinal(char c)
        {
            for (var i = 'A'; i < c; i++) yield return GenerateRow(i, c);
            for (var i = c; i >= 'A'; i--) yield return GenerateRow(i, c);
        }

        private static string GenerateRow(char currentChar, char maxChar)
        {
            var length = (maxChar - 'A' + 1) * 2 - 1;
            var padding = maxChar - currentChar;
            var sb = new StringBuilder();

            sb.Append(new string(' ', padding));
            sb.Append(currentChar);
            if (currentChar != 'A')
            {
                var insidePadding = length - padding * 2 - 2;
                sb.Append(new string(' ', insidePadding));
                sb.Append(currentChar);
            }

            sb.Append(new string(' ', padding));
            return sb.ToString();
        }
    }
}
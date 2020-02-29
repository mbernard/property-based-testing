﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using FsCheck;
using FsCheck.Xunit;

namespace Samples
{
    public class DiamondTests
    {
        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property NotEmpty(char c)
        {
            return Diamond.Generate(c).All(s => s != string.Empty).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property FirstLineContainsA(char c)
        {
            return Diamond.Generate(c).First().Contains('A').ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property LastLineContainsA(char c)
        {
            return Diamond.Generate(c).Last().Contains('A').ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property SpacesPerRowAreSymmetric(char c)
        {
            return Diamond.Generate(c).All(row =>
                CountLeadingSpaces(row) == CountTrailingSpaces(row)
            ).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property RowsContainCorrectLetterInCorrectOrder(char c)
        {
            var expected = new List<char>();
            for (var i = 'A'; i < c; i++) expected.Add(i);

            for (var i = c; i >= 'A'; i--) expected.Add(i);

            var actual = Diamond.Generate(c).ToList().Select(GetCharInRow);
            return actual.SequenceEqual(expected).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property DiamondIsSymmetric(char c)
        {
            var diamond = Diamond.Generate(c).ToList();
            return diamond.All(row => row.Length == diamond.Count).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property AllRowsExceptFirstAndLastContainTwoIdenticalLetters(char c)
        {
            if (c == 'A') return true.ToProperty();

            var diamond = Diamond.Generate(c).ToArray()[1..^1];
            return diamond.All(x =>
            {
                var s = x.Replace(" ", string.Empty);
                var b = s.Length == 2 && s.First() == s.Last();
                return b;
            }).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property SymmetricAroundHorizontalAxis(char c)
        {
            var diamond = Diamond.Generate(c).ToArray();
            var half = diamond.Length / 2;
            var topHalf = diamond[..half];
            var bottomHalf = diamond[(half + 1)..];
            return topHalf.Reverse().SequenceEqual(bottomHalf).ToProperty();
        }

        [Property(Arbitrary = new[] { typeof(LetterGenerator) })]
        public Property SymmetricAroundVerticalAxis(char c)
        {
            return Diamond.Generate(c).ToArray()
                .All(row =>
                {
                    var half = row.Length / 2;
                    var firstHalf = row[..half];
                    var secondHalf = row[(half + 1)..];
                    
                    return firstHalf.Reverse().SequenceEqual(secondHalf);
                }).ToProperty();
        }

        private int CountLeadingSpaces(string s)
        {
            return s.IndexOf(GetCharInRow(s));
        }

        private static char GetCharInRow(string s)
        {
            return s.First(x => x != ' ');
        }

        private int CountTrailingSpaces(string s)
        {
            var i = s.LastIndexOf(GetCharInRow(s));
            return s.Length - i - 1;
        }
    }

    public class Diamond
    {
        public static IEnumerable<string> Generate(char c)
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
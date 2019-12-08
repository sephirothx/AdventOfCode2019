using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    class Day8
    {
        private const int W = 25;
        private const int H = 6;
        private const int DIM = W * H;

        public static void Part1(string input)
        {
            var layers = ParseInput(input);
            var counts = new List<(int m0, int m1, int m2)>();

            foreach (string l in layers)
            {
                var m0 = Regex.Matches(l, "0").Count;
                var m1 = Regex.Matches(l, "1").Count;
                var m2 = Regex.Matches(l, "2").Count;

                counts.Add((m0, m1, m2));
            }

            (_, int i, int j) = counts.OrderBy(x => x.m0).First();

            Console.WriteLine(i * j);
        }

        public static void Part2(string input)
        {
            var layers = ParseInput(input);
            var final = layers[0].ToCharArray();

            foreach (var l in layers)
            {
                for (int i = 0; i < DIM; i++)
                {
                    if (final[i] == '2') final[i] = l[i];
                }
            }

            var ans = new string(final)
                     .Replace('1', '#')
                     .Replace('0', ' ');

            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    Console.Write(ans[i * W + j]);
                }

                Console.WriteLine();
            }
        }

        private static string[] ParseInput(string input)
        {
            var output = new string[input.Length / DIM];

            int i = 0;
            while (!string.IsNullOrEmpty(input))
            {
                output[i++] = input.Substring(0, DIM);
                input = input.Substring(DIM);
            }

            return output;
        }
    }
}
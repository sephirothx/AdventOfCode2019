using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    class Day8
    {
        private const int DIM = 25 * 6;

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
                    if (final[i] == '2')
                    {
                        final[i] = l[i];
                    }

                    final[i] = final[i] switch
                    {
                        '1' => '#',
                        '0' => ' ',
                        _   => final[i]
                    };
                }
            }

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    Console.Write(final[i*25+j]);
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
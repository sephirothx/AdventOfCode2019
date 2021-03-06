﻿using System;
using System.Numerics;
using System.Text.RegularExpressions;
using static System.Numerics.BigInteger; // ModPow

namespace AdventOfCode2019
{
    class Day22
    {
        public static void Part1(string[] input)
        {
            const int DECK_SIZE = 10007;

            var (a, b) = GetShuffleValues(input, DECK_SIZE);

            // solving a + b*x = 2019 mod DECK_SIZE
            Console.WriteLine((2019 - a) * ModInverse(b, DECK_SIZE) % DECK_SIZE);
        }

        public static void Part2(string[] input)
        {
            const long DECK_SIZE  = 119315717514047;
            const long ITERATIONS = 101741582076661;

            var values = GetShuffleValues(input, DECK_SIZE);
            var (a, b) = RepeatIteration(values, DECK_SIZE, ITERATIONS);
            Console.WriteLine((a + b * 2020) % DECK_SIZE);
        }

        private static (BigInteger a, BigInteger b) RepeatIteration((BigInteger a, BigInteger b) first_iteration_values,
                                                                    long deck_size,
                                                                    long iterations)
        {
            var (a, b) = first_iteration_values;

            // https://stackoverflow.com/a/1522895/9815377
            // Sum of a geometric sequence:
            // b^0 + b^1 + ... + b^(n-1) (mod m) = (b^n - 1)/(b - 1) (mod m)
            var seq = (ModPow(b, iterations, deck_size) - 1) * ModInverse(b - 1, deck_size) % deck_size;

            var out_a = a * seq;
            var out_b = ModPow(b, iterations, deck_size);

            return (out_a, out_b);
        }

        private static (BigInteger a, BigInteger b) GetShuffleValues(string[] input, BigInteger deck_size)
        {
            var re = new Regex(@"^(.*?)\s?(-?\d+)?$");

            BigInteger a = 0; // first index of the permutation
            BigInteger b = 1; // step increase of the permutation

            foreach (string s in input)
            {
                var match = re.Match(s);

                var arg = match.Groups[2].Success
                              ? BigInteger.Parse(match.Groups[2].Value)
                              : BigInteger.Zero;

                switch (match.Groups[1].Value)
                {
                case "cut":
                    var n = (arg + deck_size) % deck_size;
                    a = (a + n * b) % deck_size;
                    break;

                case "deal with increment":
                    n = ModInverse(arg, deck_size);
                    b = b * n % deck_size;
                    break;

                case "deal into new stack":
                    a = (a - b + deck_size) % deck_size;
                    b = deck_size - b;
                    break;
                }
            }

            return (a, b);
        }

        private static BigInteger ModInverse(BigInteger n, BigInteger mod)
        {
            return ModPow(n, mod - 2, mod);
        }
    }
}

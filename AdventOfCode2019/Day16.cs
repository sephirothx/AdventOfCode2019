using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day16
    {
        private static readonly int[] _pattern = {0, 1, 0, -1};

        public static void Part1(string input)
        {
            var numbers = new List<int>();

            foreach (char c in input)
            {
                numbers.Add(c - 0x30);
            }

            for (int i = 0; i < 100; i++)
            {
                numbers = FFT(numbers);
            }

            for (int i = 0; i < 8; i++)
            {
                Console.Write(numbers[i]);
            }

            Console.WriteLine();
        }

        public static void Part2(string input)
        {
            var numbers = new List<int>();

            for (int i = 0; i < input.Length * 10000; i++)
            {
                numbers.Add(input[i % input.Length] - 0x30);
            }

            int offset = 0;
            for (int i = 0; i < 7; i++)
            {
                offset += numbers[i] * (int)Math.Pow(10, 6 - i);
            }

            for (int j = 0; j < 100; j++)
            {
                int prev = 0;
                for (int i = numbers.Count - 1; i >= offset; i--)
                {
                    prev = numbers[i] = (numbers[i] + prev) % 10;
                }
            }

            for (int i = 0; i < 8; i++)
            {
                Console.Write(numbers[offset + i]);
            }

            Console.WriteLine();
        }

        private static List<int> FFT(List<int> numbers)
        {
            var output = new List<int>();

            for (int j = 0; j < numbers.Count; j++)
            {
                int sum = 0;

                for (int i = 0; i < numbers.Count; i++)
                {
                    sum += numbers[i] * _pattern[(i + 1) / (j + 1) % _pattern.Length];
                }

                output.Add(Math.Abs(sum % 10));
            }

            return output;
        }
    }
}

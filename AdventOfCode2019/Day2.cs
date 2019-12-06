using System;
using System.Linq;

namespace AdventOfCode2019
{
    class Day2
    {
        public static void Part1(string input)
        {
            var program = ParseInput(input);
            program[1] = 12;
            program[2] = 2;

            Intcode.Instance.Compute(program);

            Console.WriteLine(program[0]);
        }

        public static void Part2(string input)
        {
            var numbers = ParseInput(input);
            const int MAGIC_NUMBER = 19690720;

            for (int i = 0; i < numbers.Length; i++)
            for (int j = 0; j < numbers.Length; j++)
            {
                var work = new int[numbers.Length];
                Array.Copy(numbers, work, numbers.Length);

                work[1] = i;
                work[2] = j;

                Intcode.Instance.Compute(work);

                if (work[0] == MAGIC_NUMBER)
                {
                    Console.WriteLine(100 * i + j);
                    return;
                }
            }
        }

        private static int[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToArray();
        }
    }
}

using System;
using System.Linq;

namespace AdventOfCode2019
{
    class Day2
    {
        public static void Part1(string input)
        {
            var numbers = ParseInput(input);
            numbers[1] = 12;
            numbers[2] = 2;

            Console.WriteLine(Compute(numbers));
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

                if (Compute(work) == MAGIC_NUMBER)
                {
                    Console.WriteLine(100 * i + j);
                }
            }
        }

        private static int[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToArray();
        }

        private static int Compute(int[] numbers)
        {
            for (int i = 0; i < numbers.Length; i += 4)
            {
                int opcode = numbers[i];
                int num1   = numbers[i + 1];
                int num2   = numbers[i + 2];
                int target = numbers[i + 3];

                switch (opcode)
                {
                case 1:
                    numbers[target] = numbers[num1] + numbers[num2];
                    break;
                case 2:
                    numbers[target] = numbers[num1] * numbers[num2];
                    break;
                case 99:
                    return numbers[0];
                }
            }

            return 0;
        }
    }
}

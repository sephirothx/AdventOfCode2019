using System;

namespace AdventOfCode2019
{
    class Day2
    {
        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            program[1] = 12;
            program[2] = 2;

            Intcode.Instance.Compute(program);

            Console.WriteLine(program[0]);
        }

        public static void Part2(string input)
        {
            const int MAGIC_NUMBER = 19690720;

            var program = Intcode.ParseInput(input);

            for (int i = 0; i < program.Length; i++)
            for (int j = 0; j < program.Length; j++)
            {
                program[1] = i;
                program[2] = j;

                Intcode.Instance.Compute(program);

                if (Intcode.Instance.State[0] == MAGIC_NUMBER)
                {
                    Console.WriteLine(100 * i + j);
                    return;
                }
            }
        }
    }
}

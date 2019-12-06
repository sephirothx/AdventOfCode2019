using System.Linq;

namespace AdventOfCode2019
{
    class Day5
    {
        public static void Part1(string input)
        {
            var program = ParseInput(input);

            Intcode.Instance.Compute(program, 1);
        }

        public static void Part2(string input)
        {
            var program = ParseInput(input);

            Intcode.Instance.Compute(program, 5);
        }

        private static int[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToArray();
        }
    }
}

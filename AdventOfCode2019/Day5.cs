namespace AdventOfCode2019
{
    class Day5
    {
        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);

            Intcode.Instance.Compute(program, 1);
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);

            Intcode.Instance.Compute(program, 5);
        }
    }
}

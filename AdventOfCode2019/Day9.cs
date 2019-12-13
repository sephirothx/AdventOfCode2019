namespace AdventOfCode2019
{
    class Day9
    {
        public static void Part1(string input)
        {
            var p = Intcode.ParseInput(input);
            var i = new Intcode();
            i.AddInput(1);
            i.Compute(p);
        }

        public static void Part2(string input)
        {
            var p = Intcode.ParseInput(input);
            var i = new Intcode();
            i.AddInput(2);
            i.Compute(p);
        }
    }
}
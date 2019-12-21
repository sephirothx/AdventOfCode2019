using System;

namespace AdventOfCode2019
{
    class Day21
    {
        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            string INPUT = "NOT A T\nNOT B J\nOR T J\nNOT C T\nOR T J\nAND D J\nWALK\n";

            foreach (char c in INPUT)
            {
                intcode.AddInput(c);
            }

            intcode.Compute(program);
            Console.WriteLine(intcode.Output);
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            string INPUT = "NOT A T\nNOT B J\nOR T J\nNOT C T\nOR T J\nAND F T\nOR I T\nAND E T\nOR H T\nAND D T\nAND T J\nRUN\n";

            foreach (char c in INPUT)
            {
                intcode.AddInput(c);
            }

            intcode.Compute(program);
            Console.WriteLine(intcode.Output);
        }
    }
}
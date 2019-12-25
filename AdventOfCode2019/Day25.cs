using System;

namespace AdventOfCode2019
{
    class Day25
    {
        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            intcode.Compute(program, isXmas:true);
        }
    }
}
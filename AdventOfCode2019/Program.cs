using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        private static void Main()
        {
            const string PATH  = @"input.txt";
            var          input = File.ReadAllText(PATH);

            var start = DateTime.Now.TimeOfDay;
            Day11.Part2(input);
            var end = DateTime.Now.TimeOfDay;

            Console.WriteLine();
            Console.WriteLine($"Runtime = {(end - start).TotalMilliseconds}");
        }
    }
}

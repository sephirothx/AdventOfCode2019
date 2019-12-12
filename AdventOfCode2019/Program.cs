using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        private static void Main()
        {
            var input = File.ReadAllLines(Utility.INPUT_PATH);

            #if !DEBUG
            Utility.FetchInput();
            #endif

            var start = DateTime.Now.TimeOfDay;
            Day12.Part2(input);
            var end = DateTime.Now.TimeOfDay;

            Console.WriteLine();
            Console.WriteLine($"Runtime = {(end - start).TotalMilliseconds}");
        }
    }
}

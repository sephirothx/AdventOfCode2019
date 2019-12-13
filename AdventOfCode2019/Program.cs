using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        private static void Main()
        {
            var input = File.ReadAllText(Utility.INPUT_PATH);

            Utility.FetchInput();

            var start = DateTime.Now.TimeOfDay;
            Day13.Part2(input);
            var end = DateTime.Now.TimeOfDay;

            Console.WriteLine();
            Console.WriteLine($"Runtime = {(end - start).TotalMilliseconds}");
        }
    }
}

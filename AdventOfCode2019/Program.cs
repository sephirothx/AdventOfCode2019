﻿using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        private static void Main()
        {
            Utility.FetchInput();
            
            //var input = File.ReadAllLines(Utility.INPUT_PATH);
            var input = File.ReadAllText(Utility.INPUT_PATH);

            var start = DateTime.Now.TimeOfDay;
            Day25.Part1(input);
            var end = DateTime.Now.TimeOfDay;

            Console.WriteLine();
            Console.WriteLine($"Runtime = {(end - start).TotalMilliseconds}");
        }
    }
}

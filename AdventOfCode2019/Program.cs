using System;
using System.IO;

namespace AdventOfCode2019
{
    class Program
    {
        private static void Main()
        {
            const string PATH  = @"C:\Users\User\Documents\input.txt";
            var          input = File.ReadAllText(PATH);

            Console.WriteLine(DateTime.Now.TimeOfDay);
            Day5.Part2(input);
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }
    }
}

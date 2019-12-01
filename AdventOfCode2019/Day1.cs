using System.Linq;

namespace AdventOfCode2019
{
    public class Day1
    {
        public static int Part1(string[] input)
        {
            return input.Sum(s => int.Parse(s) / 3 - 2);
        }

        public static int Part2(string[] input)
        {
            int sum = 0;

            foreach (string s in input)
            {
                int fuel = int.Parse(s);

                while (fuel > 0)
                {
                    fuel =  fuel / 3 - 2;
                    sum  += fuel > 0 ? fuel : 0;
                }
            }

            return sum;
        }
    }
}

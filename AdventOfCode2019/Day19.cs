using System;

namespace AdventOfCode2019
{
    class Day19
    {
        private static long[] program;

        public static void Part1(string input)
        {
            program = Intcode.ParseInput(input);
            int ans = 0;

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    ans += IsInsideBeam(x, y);
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            int y = 100;
            int x = 0;

            for (int i = y/2; i < y*2; i++)
            {
                x = IsInsideBeam(i, y) == 1 ? i : x;
            }

            while (true)
            {
                y++;
                x += IsInsideBeam(x + 1, y);

                if (IsInsideBeam(x-99, y+99) == 1)
                {
                    Console.WriteLine($"{(x - 99) * 10000 + y}");
                    return;
                }
            }
        }

        private static int IsInsideBeam(int x, int y)
        {
            var intcode = new Intcode();
            intcode.AddInput(x);
            intcode.AddInput(y);
            intcode.Compute(program);

            return (int)intcode.Output;
        }
    }
}

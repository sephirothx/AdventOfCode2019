using System;

namespace AdventOfCode2019
{
    class Day19
    {
        private static long[] program;

        public static void Part1(string input)
        {
            program = Intcode.ParseInput(input);
            long ans = 0;

            for (int y = 0; y < 50; y++)
            {
                for (int x = 0; x < 50; x++)
                {
                    ans += IsInBeam(x, y) ? 1 : 0;
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            int count = 0;
            int step  = 1000;

            int y = 1000,
                x = 0;

            string ans = "";

            while (true)
            {
                for (int i = y / 2; i < y; i++)
                {
                    x = IsInBeam(i, y) ? i : x;
                }

                if (step == -1 && count++ > 10)
                {
                    Console.WriteLine(ans);
                    return;
                }

                if (IsInBeam(x - 99, y + 99))
                {
                    if (step == -1)
                    {
                        ans = $"{x - 99}{y}";
                    }

                    y    -= step;
                    step /= 10;
                    if (step == 0) step = -1;
                }

                y += step;
            }
        }

        private static bool IsInBeam(int x, int y)
        {
            var intcode = new Intcode();
            intcode.AddInput(x);
            intcode.AddInput(y);
            intcode.Compute(program);

            return intcode.Output == 1;
        }
    }
}

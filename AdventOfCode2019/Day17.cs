using System;
using System.Collections.Generic;
using ReStructure.Sparse;

namespace AdventOfCode2019
{
    class Day17
    {
        private static readonly SparseMatrix<char> map = new SparseMatrix<char>('.');

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            long ip = 0;
            int x = 0,
                y = 0;

            int ans = 0;

            while (true)
            {
                intcode.Compute(program, ip, false);
                if (intcode.IsOver)
                {
                    break;
                }

                if (intcode.Output == 10)
                {
                    y++;
                    x = 0;
                }
                else
                {
                    map[x++, y] = (char)intcode.Output;
                }

                program = intcode.State;
                ip      = intcode.IP;
            }

            for (x = 0; x <= map.MaxX; x++)
            {
                for (y = 0; y <= map.MaxY; y++)
                {
                    if (map[x, y] == '.')
                        continue;

                    bool test = true;
                    foreach ((int nx, int ny) in Neighbors(x, y))
                    {
                        test &= map[nx, ny] != '.';
                    }

                    ans += test ? x * y : 0;
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            program[0] = 2;

            const string INPUT = "A,B,A,C,B,C,A,B,A,C\n" + // Main:
                                 "R,6,L,10,R,8,R,8\n"    + // Function A:
                                 "R,12,L,8,L,10\n"       + // Function B:
                                 "R,12,L,10,R,6,L,10\n"  + // Function C:
                                 "n\n";                    // Continuous video feed?

            foreach (char c in INPUT)
            {
                intcode.AddInput(c);
            }

            intcode.Compute(program);
            Console.WriteLine(intcode.Output);
        }

        private static List<(int x, int y)> Neighbors(int x, int y)
        {
            var output = new List<(int x, int y)>();
            for (int i = 1; i < 5; i++)
            {
                var d = Utility.GetDirection((Direction)i);
                output.Add((x + d.x, y + d.y));
            }

            return output;
        }
    }
}

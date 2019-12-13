using System;
using System.Threading;

namespace AdventOfCode2019
{
    class Day13
    {
        // 0 is an empty tile. No game object appears in this tile.
        // 1 is a wall tile. Walls are indestructible barriers.
        // 2 is a block tile. Blocks can be broken by the ball.
        // 3 is a horizontal paddle tile. The paddle is indestructible.
        // 4 is a ball tile. The ball moves diagonally and bounces off objects.
        public static long Score { get; set; }

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);

            int ans = 0;
            var grid = new int[40, 20];

            Run(program, grid);
            foreach (int i in grid)
            {
                ans += i == 2 ? 1 : 0;
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);
            program[0] = 2;

            var grid = new int[40, 20];
            Run(program, grid);

            Console.WriteLine(Score);
        }

        private static void Run(long[] program, int[,] grid)
        {
            var intcode = new Intcode();
            long ip = 0;
            long c = 0;

            while (intcode.IsOver == false)
            {
                var o = new long[3];
                for (int i = 0; i < 3; i++)
                {
                    intcode.Compute(program, ip, false);
                    o[i]    = intcode.Output;
                    program = intcode.State;
                    ip      = intcode.IP;

                    if (intcode.IsOver)
                    {
                        break;
                    }
                }

                if (o[0] == -1)
                {
                    Score = o[2];
                    continue;
                }

                if (!intcode.IsOver)
                {
                    grid[(int)o[0], (int)o[1]] = (int)o[2];
                    if (c++ > grid.Length)
                    {
                        Console.Clear();
                        PrintGrid(grid);
                        Thread.Sleep(50);
                    }
                }
            }

        }

        private static void PrintGrid(int[,] grid)
        {
            for (int i = 0; i < grid.GetLength(1); i++)
            {
                for (int j = 0; j < grid.GetLength(0); j++)
                {
                    Console.Write(grid[j, i] switch
                    {
                        1 => '█',
                        2 => '#',
                        3 => '█',
                        4 => 'o',
                        _ => ' '
                    });
                }

                Console.WriteLine();
            }
        }
    }
}
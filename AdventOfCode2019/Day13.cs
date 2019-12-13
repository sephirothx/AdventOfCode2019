using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day13
    {
        private const int W = 40;
        private const int H = 20;

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
            var grid = new int[W, H];

            Console.CursorVisible = false;
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

            var grid = new int[W, H];
            Console.CursorVisible = false;
            Run(program, grid);
        }

        private static void Run(long[] program, int[,] grid)
        {
            var x_pos   = new Dictionary<int, int> {{3, 0}, {4, 0}};
            var intcode = new Intcode();
            long ip = 0;

            while (intcode.IsOver == false)
            {
                var o = new int[3];
                for (int i = 0; i < 3; i++)
                {
                    intcode.Compute(program, ip, false);
                    o[i]    = (int)intcode.Output;
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
                    PrintScore();
                    continue;
                }

                if (!intcode.IsOver)
                {
                    grid[o[0], o[1]] = o[2];
                    PrintGrid(o[0], o[1], o[2]);

                    x_pos[o[2]] = o[0];
                    int dist = x_pos[4] - x_pos[3];
                    intcode.Input = dist != 0 ? dist / Math.Abs(dist) : 0;
                }
            }

        }

        private static void PrintGrid(int x, int y, int type)
        {
            var bg = new[] {ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Blue, ConsoleColor.Black, ConsoleColor.Black};
            var fg = new[] {ConsoleColor.Black, ConsoleColor.Black, ConsoleColor.Red, ConsoleColor.White, ConsoleColor.Red};
            var ch = "  ×▀©";
            
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = bg[type];
            Console.ForegroundColor = fg[type];
            Console.Write(ch[type]);
        }

        private static void PrintScore()
        {
            Console.SetCursorPosition(0, H + 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Score: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Score);
            Console.Write("    ");
        }
    }
}
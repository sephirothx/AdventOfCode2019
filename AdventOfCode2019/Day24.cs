using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day24
    {
        private const int LAYERS = 201;

        private static int W, H;
        private static bool[,] grid;

        private static readonly Dictionary<(int, int), ((int, int), int)[]> inner =
            new Dictionary<(int, int), ((int, int), int)[]>
            {
                {(2, 1), new[] {((0, 0), 1), ((1, 0), 1), ((2, 0), 1), ((3, 0), 1), ((4, 0), 1)}},
                {(2, 3), new[] {((0, 4), 1), ((1, 4), 1), ((2, 4), 1), ((3, 4), 1), ((4, 4), 1)}},
                {(1, 2), new[] {((0, 0), 1), ((0, 1), 1), ((0, 2), 1), ((0, 3), 1), ((0, 4), 1)}},
                {(3, 2), new[] {((4, 0), 1), ((4, 1), 1), ((4, 2), 1), ((4, 3), 1), ((4, 4), 1)}}
            };

        public static void Part1(string[] input)
        {
            W = input[0].Length;
            H = input.Length;
            grid = ParseInput(input);

            var states = new HashSet<long>();

            while (true)
            {
                long biodiversity = CalculateBiodiversity();

                if (states.Contains(biodiversity))
                {
                    Console.WriteLine(biodiversity);
                    return;
                }

                states.Add(biodiversity);
                GameOfLife();
            }
        }

        public static void Part2(string[] input)
        {
            var bigGrid = new bool[LAYERS][,];
            for (int i = 0; i < LAYERS; i++)
            {
                bigGrid[i] = new bool[W, H];
            }

            bigGrid[LAYERS / 2] = ParseInput(input);

            for (int i = 0; i < 200; i++)
            {
                bigGrid = GameOfLife2(bigGrid);
            }

            int ans = 0;
            foreach (var g in bigGrid)
            {
                foreach (bool b in g)
                {
                    ans += b ? 1 : 0;
                }
            }

            Console.WriteLine(ans);
        }

        private static bool[,] ParseInput(string[] input)
        {
            var output = new bool[W, H];

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    output[x, y] = input[y][x] == '#';
                }
            }

            return output;
        }

        private static bool[][,] GameOfLife2(bool[][,] g)
        {
            var newBigGrid = new bool[LAYERS][,];
            for (int i = 0; i < LAYERS; i++)
            {
                newBigGrid[i] = new bool[W, H];
            }

            for (int level = 0; level < LAYERS; level++)
            {
                for (int y = 0; y < H; y++)
                {
                    for (int x = 0; x < W; x++)
                    {
                        if (x == 2 && y == 2)
                            continue;

                        int neighbors = 0;

                        foreach (var n in GetNeighbors2((x, y)))
                        {
                            if (level + n.l < 0 || level + n.l >= LAYERS)
                                continue;

                            if (g[level + n.l][n.cell.x, n.cell.y])
                            {
                                neighbors++;
                            }
                        }

                        if (g[level][x, y] && neighbors != 1)
                        {
                            newBigGrid[level][x, y] = false;
                        }
                        else if (g[level][x, y] == false && (neighbors == 1 || neighbors == 2))
                        {
                            newBigGrid[level][x, y] = true;
                        }
                        else
                        {
                            newBigGrid[level][x, y] = g[level][x, y];
                        }
                    }
                }
            }

            return newBigGrid;
        }

        private static List<((int x, int y) cell, int l)> GetNeighbors2((int x, int y) p)
        {
            var output = new List<((int, int), int)>();
            for (int i = 1; i < 5; i++)
            {
                (int dx, int dy) = Utility.GetDirection((Direction)i);
                (int x, int y) n;
                n.x = p.x + dx;
                n.y = p.y + dy;

                switch (n)
                {
                case (-1, _):
                    output.Add(((1, 2), -1));
                    break;
                case (_, -1):
                    output.Add(((2, 1), -1));
                    break;
                case (5, _):
                    output.Add(((3, 2), -1));
                    break;
                case (_, 5):
                    output.Add(((2, 3), -1));
                    break;
                case (2, 2):
                    output.AddRange(inner[p]);
                    break;
                default:
                    output.Add(((n), 0));
                    break;
                }
            }

            return output;
        }

        private static void GameOfLife()
        {
            var newgrid = new bool[grid.GetLength(0), grid.GetLength(1)];

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    int neighbors = 0;

                    foreach (var n in GetNeighbors((x, y)))
                    {
                        if (n.x < 0 || n.y < 0 || n.x >= W || n.y >= H)
                            continue;

                        if (grid[n.x, n.y])
                        {
                            neighbors++;
                        }
                    }

                    if (grid[x, y] && neighbors != 1)
                    {
                        newgrid[x, y] = false;
                    }
                    else if (grid[x, y] == false && (neighbors == 1 || neighbors == 2))
                    {
                        newgrid[x, y] = true;
                    }
                    else
                    {
                        newgrid[x, y] = grid[x, y];
                    }
                }
            }

            grid = newgrid;
        }

        private static List<(int x, int y)> GetNeighbors((int x, int y) p)
        {
            var output = new List<(int, int)>();
            for (int i = 1; i < 5; i++)
            {
                (int dx, int dy) = Utility.GetDirection((Direction)i);
                (int x, int y) next;
                next.x = p.x + dx;
                next.y = p.y + dy;

                output.Add(next);
            }

            return output;
        }

        private static long CalculateBiodiversity()
        {
            long sum = 0;

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    if (grid[x, y])
                    {
                        sum += (long)Math.Pow(2, y * grid.GetLength(0) + x);
                    }
                }
            }

            return sum;
        }
    }
}

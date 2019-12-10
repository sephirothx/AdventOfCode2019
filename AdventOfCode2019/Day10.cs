using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day10
    {
        public static void Part1(string[] input)
        {
            int W = input[0].Length;
            int H = input.Length;
            var grid = new int[W, H];

            for (int i = 0; i < H; i++)
            {
                string s = input[i];

                for (int j = 0; j < W; j++)
                {
                    grid[j, i] = s[j] == '.' ? -1 : 0;
                }
            }

            int ans = 0;
            int x = 0,
                y = 0;

            for (int i = 0; i < H; i++)
            {
                for (int j = 0; j < W; j++)
                {
                    if (grid[j, i] < 0) continue;

                    var set = new HashSet<(int, int)>();

                    for (int k = 0; k < H; k++)
                    {
                        for (int l = 0; l < W; l++)
                        {
                            if ((i, j) == (k, l) || grid[l, k] < 0) continue;

                            set.Add(Minterms(i-k, j-l));
                        }
                    }

                    grid[j, i] = set.Count;
                    if (set.Count > ans)
                    {
                        ans = set.Count;
                        x = j;
                        y = i;
                    }
                }
            }

            Console.WriteLine($"{x},{y} - {ans}");
        }

        private static (int, int) Minterms(int x, int y)
        {
            if (x == 0 || y == 0)
            {
                return (x == 0 ? 0 : x / Math.Abs(x),
                        y == 0 ? 0 : y / Math.Abs(y));
            }

            int a = Math.Abs(x),
                b = Math.Abs(y);
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            int gcd = a == 0 ? b : a;

            return (x / gcd, y / gcd);
        }
    }
}
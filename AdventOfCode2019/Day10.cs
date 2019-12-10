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

            var asteroids = new HashSet<(int x, int y)>();

            for (int i = 0; i < H; i++)
            {
                string s = input[i];

                for (int j = 0; j < W; j++)
                {
                    if (s[j] == '#')
                        asteroids.Add((j, i));
                }
            }

            int ans = 0;
            int x   = 0,
                y   = 0;

            foreach (var center in asteroids)
            {
                var seen = new HashSet<(int, int)>();

                foreach (var target in asteroids)
                {
                    if (target == center) continue;

                    seen.Add(Minterms(target.x - center.x, target.y - center.y));
                }

                if (seen.Count > ans)
                {
                    ans = seen.Count;
                    x   = center.x;
                    y   = center.y;
                }
            }

            Console.WriteLine($"{x},{y} - {ans}");
        }

        private static (int, int) Minterms(int x, int y)
        {
            int a = Math.Abs(x),
                b = Math.Abs(y);

            if (x == 0 || y == 0)
            {
                return (x == 0 ? 0 : x / a,
                        y == 0 ? 0 : y / b);
            }

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
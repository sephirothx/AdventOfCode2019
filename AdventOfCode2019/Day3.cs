using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    class Day3
    {
        private static readonly Dictionary<char, int> _dx = new Dictionary<char, int> {{'R', 1}, {'L', -1}, {'D', 0}, {'U', 0}};
        private static readonly Dictionary<char, int> _dy = new Dictionary<char, int> {{'R', 0}, {'L', 0}, {'D', 1}, {'U', -1}};

        public static void Part1(string[] input)
        {
            var grid = new HashSet<(int, int)>();

            int ans = int.MaxValue;

            for (int j = 0; j < 2; j++)
            {
                var orders = input[j].Split(',');
                
                int x = 0;
                int y = 0;
                
                foreach (string order in orders)
                {
                    char dir = order[0];
                    int amount = int.Parse(order.Substring(1));

                    for (int i = 0; i < amount; i++)
                    {
                        x += _dx[dir];
                        y += _dy[dir];

                        if (j == 0)
                        {
                            grid.Add((x, y));
                        }
                        else if (grid.Contains((x, y)))
                        {
                            ans = Math.Min(ans, Math.Abs(x) + Math.Abs(y));
                        }
                    }
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string[] input)
        {
            var path = new Dictionary<(int, int), int>();

            int ans = int.MaxValue;

            for (int j = 0; j < 2; j++)
            {
                var orders = input[j].Split(',');

                int x = 0;
                int y = 0;
                int l = 0;

                foreach (string order in orders)
                {
                    char dir    = order[0];
                    int  amount = int.Parse(order.Substring(1));

                    for (int i = 0; i < amount; i++)
                    {
                        x += _dx[dir];
                        y += _dy[dir];

                        l++;

                        if (j == 0)
                        {
                            path.TryAdd((x, y), l);
                        }
                        else if (path.ContainsKey((x,y)))
                        {
                            ans = Math.Min(path[(x, y)] + l, ans);
                        }
                    }
                }
            }

            Console.WriteLine(ans);
        }
    }
}

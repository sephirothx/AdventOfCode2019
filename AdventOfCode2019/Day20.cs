using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure;

// ReSharper disable CollectionNeverUpdated.Local

namespace AdventOfCode2019
{
    using Gates = DefaultDictionary<string, List<(int x, int y)>>;
    using Portals = Dictionary<(int x, int y), (int x, int y)>;

    class Day20
    {
        private const char PATH = '.';

        private static int W, H;
        private static char[,] map;

        private static readonly Portals portals = new Portals();

        private static (int x, int y) start, end;

        public static void Part1(string[] input)
        {
            ParseInput1(input);
            Console.WriteLine(ShortestPath(start));
        }

        public static void Part2(string[] input)
        {
            Console.WriteLine(ShortestPath2(start, 0));
        }

        private static int ShortestPath((int x, int y) p)
        {
            var queue    = new Queue<(int, int)>();
            var distance = new Dictionary<(int, int), int> {{p, 0}};

            queue.Enqueue(p);
            while (queue.Any())
            {
                var curr = queue.Dequeue();

                if (curr == end)
                {
                    return distance[curr];
                }

                foreach (var n in GetNeighbors(curr))
                {
                    if (distance.ContainsKey(n))
                        continue;

                    if (map[n.x, n.y] == PATH)
                    {
                        distance.Add(n, distance[curr] + 1);
                        queue.Enqueue(n);
                    }
                    else if (IsPortal(map[n.x, n.y]))
                    {
                        if (!portals.ContainsKey(curr))
                            continue;

                        var next = portals[curr];

                        if (!distance.ContainsKey(next))
                        {
                            distance.Add(next, distance[curr] + 1);
                            queue.Enqueue(next);
                        }
                    }
                }
            }

            return int.MaxValue;
        }

        private static int ShortestPath2((int x, int y) p, int level)
        {
            var queue    = new Queue<((int x, int y) p, int l)>();
            var distance = new Dictionary<((int, int), int), int> {{(p, level), 0}};

            var goal = (end, 0);

            queue.Enqueue((p, level));
            while (queue.Any())
            {
                var curr = queue.Dequeue();

                if (curr == goal)
                {
                    return distance[curr];
                }

                foreach (var n in GetNeighbors(curr.p))
                {
                    if (map[n.x, n.y] == PATH)
                    {
                        if (!distance.ContainsKey((n, curr.l)))
                        {
                            distance.Add((n, curr.l), distance[curr] + 1);
                            queue.Enqueue((n, curr.l));
                        }
                    }
                    else if (IsPortal(map[n.x, n.y]))
                    {
                        if (!portals.ContainsKey(curr.p))
                            continue;

                        var next = portals[curr.p];
                        int l    = curr.l + Jump(curr.p);

                        if (l >= 0 && !distance.ContainsKey((next, l)))
                        {
                            distance.Add((next, l), distance[curr] + 1);
                            queue.Enqueue((next, l));
                        }
                    }
                }
            }

            return int.MaxValue;
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

        private static void ParseInput1(string[] input)
        {
            W = input[0].Length;
            H = input.Length;

            map = new char[W, H];

            var gates = new Gates();

            for (int y = 0; y < H; y++)
            {
                for (int x = 0; x < W; x++)
                {
                    map[x, y] = input[y][x];

                    if (map[x, y] != PATH)
                        continue;

                    string key = IsPortal(input[y][x-1]) ? $"{input[y][x-2]}{input[y][x-1]}" :
                                 IsPortal(input[y-1][x]) ? $"{input[y-2][x]}{input[y-1][x]}" :
                                 IsPortal(input[y][x+1]) ? $"{input[y][x+1]}{input[y][x+2]}" :
                                 IsPortal(input[y+1][x]) ? $"{input[y+1][x]}{input[y+2][x]}" : 
                                                           null;
                    if (key != null)
                        gates[key].Add((x, y));
                }
            }

            foreach (var (_, pos) in gates)
            {
                if (pos.Count > 1)
                {
                    portals[pos[0]] = pos[1];
                    portals[pos[1]] = pos[0];
                }
            }

            start = gates["AA"][0];
            end   = gates["ZZ"][0];
        }

        private static int Jump((int x, int y) p)
        {
            return (p.x == 2 || p.y == 2 || p.x == W - 3 || p.y == H - 3) ? -1 : 1;
        }

        private static bool IsPortal(char c)
        {
            return c >= 'A' && c <= 'Z';
        }
    }
}

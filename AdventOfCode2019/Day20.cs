using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure;

// ReSharper disable CollectionNeverUpdated.Local

namespace AdventOfCode2019
{
    using Portals = DefaultDictionary<int, List<(int x, int y)>>;
    using Connections = Dictionary<(int x, int y), (int x, int y)>;
    using Jumps = Dictionary<(int x, int y), int>;

    class Day20
    {
        private const int EMPTY = 32;
        private const int PATH  = 46;
        private const int WALL  = 35;

        private static int[,] map;

        private static readonly Portals     portals     = new Portals();
        private static readonly Connections connections = new Connections();
        private static readonly Jumps       jumps       = new Jumps();

        public static void Part1(string[] input)
        {
            ParseInput(input);
            ConnectPortals();

            var p = portals[6565][0];
            Console.WriteLine(ShortestPath(p));
        }

        public static void Part2(string[] input)
        {
            var p = portals[6565][0];
            Console.WriteLine(ShortestPath2(p, 0));
        }

        private static int ShortestPath((int x, int y) p)
        {
            var queue    = new Queue<(int, int)>();
            var distance = new Dictionary<(int, int), int> {{p, 0}};

            queue.Enqueue(p);
            while (queue.Any())
            {
                var curr = queue.Dequeue();

                if (curr == portals['Z' * 100 + 'Z'][0])
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
                    else if (map[n.x, n.y] > 999)
                    {
                        try
                        {
                            var next = connections[curr];

                            if (!distance.ContainsKey(next))
                            {
                                distance.Add(next, distance[curr] + 1);
                                queue.Enqueue(next);
                            }
                        }
                        catch (Exception)
                        {
                            // ignore
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

            var goal = (portals['Z' * 100 + 'Z'][0], 0);

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
                    else if (map[n.x, n.y] > 999)
                    {
                        try
                        {
                            var next = connections[curr.p];
                            int l    = curr.l + jumps[curr.p];

                            if (l >= 0 && !distance.ContainsKey((next, l)))
                            {
                                distance.Add((next, l), distance[curr] + 1);
                                queue.Enqueue((next, l));
                            }
                        }
                        catch (Exception)
                        {
                            // ignore
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

        private static void ConnectPortals()
        {
            foreach (var portal in portals)
            {
                if (portal.Value.Count > 1)
                {
                    connections[portal.Value[0]] = portal.Value[1];
                    connections[portal.Value[1]] = portal.Value[0];
                }
            }
        }

        private static void ParseInput(string[] input)
        {
            map = new int[input[0].Length, input.Length];

            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    map[x, y] = input[y][x];
                }
            }

            for (int i = 0; i < map.GetLength(0); i++)
            {
                if (map[i, 0] > PATH)
                {
                    map[i, 1] = map[i, 0] * 100 + map[i, 1];
                    portals[map[i, 1]].Add((i, 2));
                    jumps[(i, 2)] = -1;
                }

                if (map[i, 86] > PATH)
                {
                    map[i, 87] = map[i, 86] * 100 + map[i, 87];
                    portals[map[i, 87]].Add((i, 88));
                    jumps[(i, 88)] = 1;
                }

                if (map[i, 33] > PATH)
                {
                    map[i, 33] = map[i, 33] * 100 + map[i, 34];
                    portals[map[i, 33]].Add((i, 32));
                    jumps[(i, 32)] = 1;
                }

                if (map[i, 119] > PATH)
                {
                    map[i, 119] = map[i, 119] * 100 + map[i, 120];
                    portals[map[i, 119]].Add((i, 118));
                    jumps[(i, 118)] = -1;
                }
            }

            for (int i = 0; i < map.GetLength(1); i++)
            {
                if (map[0, i] > PATH)
                {
                    map[1, i] = map[0, i] * 100 + map[1, i];
                    portals[map[1, i]].Add((2, i));
                    jumps[(2, i)] = -1;
                }

                if (map[90, i] > PATH)
                {
                    map[91, i] = map[90, i] * 100 + map[91, i];
                    portals[map[91, i]].Add((92, i));
                    jumps[(92, i)] = 1;
                }

                if (map[33, i] > PATH)
                {
                    map[33, i] = map[33, i] * 100 + map[34, i];
                    portals[map[33, i]].Add((32, i));
                    jumps[(32, i)] = 1;
                }

                if (map[123, i] > PATH)
                {
                    map[123, i] = map[123, i] * 100 + map[124, i];
                    portals[map[123, i]].Add((122, i));
                    jumps[(122, i)] = -1;
                }
            }
        }
    }
}

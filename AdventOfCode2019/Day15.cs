using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure.Queues;
using ReStructure.Sparse;

namespace AdventOfCode2019
{
    class Day15
    {
        private static SparseMatrix<long>  _map     = new SparseMatrix<long>(-1);
        private static HashSet<(int, int)> _visited = new HashSet<(int, int)>();

        private static Intcode _drone;

        private static (int x, int y) _target_pos = (0, 0);

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            _drone = new Intcode();

            Explore(program, 0, 0, 0);
            Console.WriteLine(ShortestPathLength((0, 0)));
        }

        public static void Part2(string input)
        {
            PrintMaze();
            Console.WriteLine(BFS(_target_pos.x, _target_pos.y));
        }

        private static void Explore(long[] program, long ip, int x, int y)
        {
            if (_visited.Contains((x, y)) ||
                _map[x, y] == 0)
            {
                return;
            }

            if (_map[x, y] == 2)
            {
                _target_pos = (x, y);
            }

            _visited.Add((x, y));

            for (int i = 1; i < 5; i++)
            {
                _drone.Input = i;

                _drone.Compute(program, ip, false);
                (int dx, int dy) = Utility.GetDirection((Direction)i);
                (int x, int y) next = (x + dx, y + dy);
                _map[next.x, next.y] = _drone.Output;

                Explore(_drone.State, _drone.IP, next.x, next.y);
            }
        }

        private static int BFS(int x, int y)
        {
            var queue    = new Queue<(int, int)>();
            var distance = new Dictionary<(int, int), int> {{(x, y), 0}};

            int max = 0;

            queue.Enqueue((x, y));
            while (queue.Any())
            {
                var curr = queue.Dequeue();
                max = Math.Max(max, distance[curr]);

                foreach (var n in GetNeighbors(curr))
                {
                    if (!distance.ContainsKey(n) &&
                        (_map[n.x, n.y] == 1 ||
                         _map[n.x, n.y] == 2))
                    {
                        distance.Add(n, distance[curr] + 1);
                        queue.Enqueue(n);
                    }
                }
            }

            return max;
        }

        private static int ShortestPathLength((int x, int y) start)
        {
            var unvisited = new PriorityQueue<(int x, int y)>(PriorityQueueOrder.Descending);
            var visited   = new HashSet<(int x, int y)>();

            unvisited.Push(start, 0);

            while (unvisited.IsEmpty == false)
            {
                var curr = unvisited.Pop();

                var node     = curr.Value;
                int priority = curr.Priority;

                if (_map[node.x, node.y] == 2)
                {
                    return priority;
                }

                if (visited.Contains(node)) continue;

                visited.Add(node);

                foreach (var n in GetNeighbors(node))
                {
                    if (visited.Contains(n)) continue;

                    if (_map[n.x, n.y] > 0)
                        unvisited.Push(n, priority + 1);
                }
            }

            return int.MaxValue;
        }

        private static (int x, int y)[] GetNeighbors((int x, int y) pos)
        {
            var output = new List<(int, int)>();
            for (int i = 1; i < 5; i++)
            {
                (int dx, int dy) = Utility.GetDirection((Direction)i);
                (int x, int y) next;
                next.x = pos.x + dx;
                next.y = pos.y + dy;

                output.Add(next);
            }

            return output.ToArray();
        }

        private static void PrintMaze()
        {
            for (int y = _map.MinY; y <= _map.MaxY; y++)
            {
                for (int x = _map.MinX; x <= _map.MaxX; x++)
                {
                    if (x == 0 &&
                        y == 0)
                    {
                        Console.Write('O');
                        continue;
                    }

                    Console.Write(_map[x, y] switch
                    {
                        0 => '█',
                        1 => ' ',
                        2 => 'X',
                        _ => ' '
                    });
                }

                Console.WriteLine();
            }
        }
    }
}

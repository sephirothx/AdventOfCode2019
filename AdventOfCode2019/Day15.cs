using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure.Queues;
using ReStructure.Sparse;

namespace AdventOfCode2019
{
    class Day15
    {
        private static SparseMatrix<long>  _map     = new SparseMatrix<long>(3);
        private static HashSet<(int, int)> _visited = new HashSet<(int, int)>();

        private static Intcode _drone;

        private static (int x, int y) _target_pos = (0, 0);

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            _drone = new Intcode();

            Explore(program, 0, 0, 0);
            Console.WriteLine(AStar((0, 0)));
        }

        public static void Part2(string input)
        {
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

        public static int AStar((int x, int y) start)
        {
            var unvisited = new PriorityQueue<((int x, int y) pos, int dist)>(PriorityQueueOrder.Descending);
            var visited   = new HashSet<(int x, int y)>();

            unvisited.Push((start, 0), 0);

            while (unvisited.IsEmpty == false)
            {
                var curr = unvisited.Pop();
                var node = curr.Value;

                if (_map[node.pos.x, node.pos.y] == 2)
                {
                    return node.dist;
                }

                if (visited.Contains(node.pos)) continue;

                visited.Add(node.pos);

                foreach (var n in GetNeighbors(node.pos))
                {
                    if (visited.Contains(n)) continue;

                    if (_map[n.x, n.y] > 0)
                        unvisited.Push((n, node.dist + 1),
                                       Utility.ManhattanDistance(_target_pos, n));
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

        private static void PrintCell(int x, int y, ConsoleColor color)
        {
            Console.SetCursorPosition(x - _map.MinX, y - _map.MinY);
            Console.BackgroundColor = color;
            Console.Write(' ');

            Console.ResetColor();
        }

        private static void PrintMaze()
        {
            var bg = new[] {ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.Blue, ConsoleColor.Gray};

            for (int y = _map.MinY; y <= _map.MaxY; y++)
            {
                for (int x = _map.MinX; x <= _map.MaxX; x++)
                {
                    PrintCell(x, y, bg[_map[x, y]]);
                }

                Console.WriteLine();
            }
        }
    }
}

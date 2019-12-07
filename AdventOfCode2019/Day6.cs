using System;
using System.Collections.Generic;
using System.Linq;
using ReStructure.Queues;

namespace AdventOfCode2019
{
    class Day6
    {
        private class Node
        {
            public Node Parent { get; set; }

            private int? _ancestors;
            public int Ancestors => (_ancestors ??= Parent?.Ancestors + 1) ?? 0;

            public List<Node> Children { get; } = new List<Node>();

            public List<Node> Neighbours
            {
                get
                {
                    var output = new List<Node>();

                    output.AddRange(Children);
                    if (Parent != null)
                    {
                        output.Add(Parent);
                    }

                    return output;
                }
            }
        }

        private const string YOU   = "YOU";
        private const string SANTA = "SAN";

        public static void Part1(string[] input)
        {
            var sat = ParseInput(input).Values;
            int sum = sat.Sum(satellite => satellite.Ancestors);

            Console.WriteLine(sum);
        }

        public static void Part2(string[] input)
        {
            var sat = ParseInput(input);

            var you   = sat[YOU];
            var santa = sat[SANTA];

            Console.WriteLine(ShortestPathLength(you, santa) - 2);
        }

        private static int ShortestPathLength(Node start, Node destination)
        {
            var unvisited = new PriorityQueue<Node>(PriorityQueueOrder.Descending);
            var visited   = new HashSet<Node>();

            unvisited.Push(start, 0);

            while (unvisited.IsEmpty == false)
            {
                var curr = unvisited.Pop();

                var node     = curr.Value;
                int priority = curr.Priority;

                if (node == destination)
                {
                    return priority;
                }

                if (visited.Contains(node)) continue;

                visited.Add(node);

                foreach (var n in node.Neighbours)
                {
                    if (visited.Contains(n)) continue;

                    unvisited.Push(n, priority + 1);
                }
            }

            return int.MaxValue;
        }

        private static Dictionary<string, Node> ParseInput(string[] input)
        {
            var nodes = new Dictionary<string, Node> {{"COM", new Node()}};

            foreach (string s in input)
            {
                var edge = s.Split(')');

                if (!nodes.TryGetValue(edge[0], out var parent))
                {
                    parent = new Node();
                    nodes.Add(edge[0], parent);
                }

                if (!nodes.TryGetValue(edge[1], out var child))
                {
                    child = new Node();
                    nodes.Add(edge[1], child);
                }

                child.Parent = parent;
                parent.Children.Add(child);
            }

            return nodes;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            var sats = new Dictionary<string, Node> {{"COM", new Node()}};

            foreach (string s in input)
            {
                var regex = new Regex(@"(.+)\)(.+)");
                var match = regex.Match(s);

                string par = match.Groups[1].Value;
                string sat = match.Groups[2].Value;

                if (!sats.TryGetValue(par, out var parent))
                {
                    parent = new Node();
                    sats.Add(par, parent);
                }

                if (!sats.TryGetValue(sat, out var child))
                {
                    child = new Node();
                    sats.Add(sat, child);
                }

                child.Parent = parent;
                parent.Children.Add(child);
            }

            return sats;
        }
    }
}
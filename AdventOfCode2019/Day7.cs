using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day7
    {
        public static void Part1(string input)
        {
            var a = new[] {0, 1, 2, 3, 4};

            var amps = new Intcode[5];
            int ans  = int.MinValue;

            var program = ParseInput(input);
            
            for (int i = 0; i < amps.Length; i++)
            {
                amps[i] = new Intcode();
            }

            foreach (var p in Permutations(a, 0, a.Length-1))
            {
                int signal = 0;
                for (int i = 0; i < p.Length; i++)
                {
                    amps[i].Input = p[i];
                    amps[i].Input = signal;
                    amps[i].Compute(program);
                    signal = amps[i].Output;
                }

                ans = Math.Max(ans, signal);
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            var a = new[] {5, 6, 7, 8, 9};

            var amps = new Intcode[5];
            int ans  = int.MinValue;

            var program = new int[5][];
            var ip = new int[5];
            
            for (int i = 0; i < amps.Length; i++)
            {
                amps[i] = new Intcode();
            }

            foreach (var p in Permutations(a, 0, a.Length -1))
            {
                int signal = 0;

                for (int i = 0; i < p.Length; i++)
                {
                    program[i]    = ParseInput(input);
                    ip[i]         = 0;
                    amps[i].Flush();
                    amps[i].Input = p[i];
                }

                bool go = true;
                while (go)
                {
                    for (int i = 0; i < p.Length; i++)
                    {
                        amps[i].Input = signal;
                        amps[i].Compute(program[i], ip[i], false);
                        signal = amps[i].Output;

                        program[i] = amps[i].Program;
                        ip[i]      = amps[i].IP;
                    }

                    go = amps[0].IsOver == false;
                }

                ans = Math.Max(ans, signal);
            }

            Console.WriteLine(ans);
            //var a = new[] {0, 1, 2, 3, 4};

            //var amps = new Intcode[5];
            //int ans  = int.MinValue;

            //var program = new int[5][];
            //var ips     = new int[5];

            //for (int i = 0; i < amps.Length; i++)
            //{
            //    amps[i] = new Intcode();
            //}

            //foreach (var p in Permutations(a, 0, a.Length-1))
            //{
            //    int signal = 0;

            //    for (int i = 0; i < p.Length; i++)
            //    {
            //        program[i] = ParseInput(input);
            //        ips[i] = 0;
            //        amps[i].Input = p[i];
            //    }

            //    bool go = true;
            //    while (go)
            //    {
            //        for (int i = 0; i < p.Length; i++)
            //        {
            //            amps[i].Input = signal;
            //            amps[i].Compute(program[i], false, ips[i]);

            //            signal = amps[i].Output;

            //            program[i] = amps[i].Program;
            //            ips[i]     = amps[i].IP;

            //            go = amps[i].IsOver == false;
            //        }

            //        ans = Math.Max(ans, signal);
            //    }
            //}

            //Console.WriteLine(ans);
        }

        public static void Swap (ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        private static IEnumerable<int[]> Permutations(int[] list, int k, int m)
        {
            if (k == m)
            {
                yield return list;
            }
            else
            {
                for (int i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    foreach (var l in Permutations(list, k + 1, m))
                    {
                        yield return l;
                    }
                    Swap(ref list[k], ref list[i]);
                }
            }
        }

        private static int[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToArray();
        }
    }
}
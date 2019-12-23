using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day23
    {
        public static void Part1(string input)
        {
            var nics = new Intcode[50];
            var program = new long[50][];
            var ip = new long[50];
            for (int i = 0; i < 50; i++)
            {
                nics[i] = new Intcode();
                nics[i].Input = i;

                program[i] = Intcode.ParseInput(input);
            }

            while (true)
            {
                for (int i = 0; i < 50; i++)
                {
                    nics[i].Compute(program[i], ip[i], true, true);
                    program[i] = nics[i].State;
                    ip[i] = nics[i].IP;

                    while (nics[i].Outputs.Any())
                    {
                        long dst = nics[i].Outputs.Dequeue();
                        long x = nics[i].Outputs.Dequeue();
                        long y = nics[i].Outputs.Dequeue();

                        if (dst == 255)
                        {
                            Console.WriteLine((x, y));
                            return;
                        }

                        nics[dst].AddInput(x);
                        nics[dst].AddInput(y);
                    }
                }
            }
        }

        public static void Part2(string input)
        {
            var nics    = new Intcode[50];
            var program = new long[50][];
            var ip      = new long[50];
            for (int i = 0; i < 50; i++)
            {
                nics[i]       = new Intcode();
                nics[i].Input = i;

                program[i] = Intcode.ParseInput(input);
            }

            var idle = new HashSet<int>();
            var sent = new HashSet<(long, long)>();
            (long x, long y) nat = (-1, -1);

            while (true)
            {
                if (idle.Count == 50 && nat != (-1, -1))
                {
                    if (sent.Contains(nat))
                    {
                        Console.WriteLine(nat);
                        return;
                    }
                
                    nics[0].AddInput(nat.x);
                    nics[0].AddInput(nat.y);
                    sent.Add(nat);
                }

                for (int i = 0; i < 50; i++)
                {
                    if (!nics[i].HasInput())
                    {
                        idle.Add(i);
                    }
                    else if (idle.Contains(i))
                    {
                        idle.Remove(i);
                    }

                    nics[i].Compute(program[i], ip[i], true, true);
                    program[i] = nics[i].State;
                    ip[i]      = nics[i].IP;

                    while (nics[i].Outputs.Any())
                    {
                        long dst = nics[i].Outputs.Dequeue();
                        long x   = nics[i].Outputs.Dequeue();
                        long y   = nics[i].Outputs.Dequeue();

                        if (dst == 255)
                        {
                            nat = (x, y);
                            continue;
                        }

                        nics[dst].AddInput(x);
                        nics[dst].AddInput(y);
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2019
{
    class Day12
    {
        public static void Part1(string[] input)
        {
            var moons     = ParseInput(input);
            var moons_vel = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                moons_vel[i] = new[] {0, 0, 0};
            }

            for (int i = 0; i < 1000; i++)
            {
                SimulateStep(moons, moons_vel);
            }

            Console.WriteLine(CalculateEnergy(moons, moons_vel));
        }

        public static void Part2(string[] input)
        {
            var moons     = ParseInput(input);
            var moons_vel = new int[4][];
            for (int i = 0; i < 4; i++)
            {
                moons_vel[i] = new[] {0, 0, 0};
            }

            var set = new Dictionary<(int, int, int, int, int, int, int, int), int>[3];
            for (int i = 0; i < 3; i++)
            {
                set[i] = new Dictionary<(int, int, int, int, int, int, int, int), int>();
                var s = GetState(i, moons, moons_vel);
                set[i][s] = 0;
            }

            var hasRepeated = new[] {false, false, false};
            var rep         = new long[3];

            for (int i = 1; i < 1000000000; i++)
            {
                SimulateStep(moons, moons_vel);

                bool stop = true;
                for (int j = 0; j < 3; j++)
                {
                    var s = GetState(j, moons, moons_vel);
                    if (set[j].ContainsKey(s) &&
                        hasRepeated[j] == false)
                    {
                        Console.WriteLine($"[{j}] : {i} - {set[j][s]}");
                        hasRepeated[j] = true;
                        rep[j]         = i;
                    }
                    else if (hasRepeated[j] == false)
                    {
                        set[j][s] = i;
                    }

                    stop &= hasRepeated[j];
                }

                if (stop) break;
            }

            Console.WriteLine();
            Console.WriteLine(Utility.LCM(rep[0], Utility.LCM(rep[1], rep[2])));
        }

        private static (int, int, int, int, int, int, int, int) GetState(int i, int[][] moons, int[][] moons_vel)
        {
            var s = (moons[0][i], moons[1][i], moons[2][i], moons[3][i],
                     moons_vel[0][i], moons_vel[3][i], moons_vel[2][i], moons_vel[3][i]);

            return s;
        }

        private static void PrintMoons(int[][] moons, int[][] moons_vel)
        {
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine($"pos=<x={moons[i][0],3}, y={moons[i][1],3}, z={moons[i][2],3}>, " +
                                  $"vel=<x={moons_vel[i][0],3}, y={moons_vel[i][1],3}, z={moons_vel[i][2],3}>");
            }
        }

        private static int CalculateEnergy(int[][] moons, int[][] moons_vel)
        {
            int energy = 0;

            for (int i = 0; i < 4; i++)
            {
                int pot = 0;
                int kin = 0;

                for (int j = 0; j < 3; j++)
                {
                    pot += Math.Abs(moons[i][j]);
                    kin += Math.Abs(moons_vel[i][j]);
                }

                energy += pot * kin;
            }

            return energy;
        }

        private static void SimulateStep(int[][] moons, int[][] moons_vel)
        {
            ApplyGravity(moons, moons_vel);
            MoveMoons(moons, moons_vel);
        }

        private static void MoveMoons(int[][] moons, int[][] moons_vel)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    moons[i][j] += moons_vel[i][j];
                }
            }
        }

        private static void ApplyGravity(int[][] moons, int[][] moons_vel)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Gravity(moons[i], moons[j], moons_vel[i]);
                }
            }
        }

        private static void Gravity(int[] pos_a, int[] pos_b, int[] vel_a)
        {
            for (int i = 0; i < pos_a.Length; i++)
            {
                vel_a[i] += pos_a[i] > pos_b[i] ? -1 :
                            pos_a[i] < pos_b[i] ? 1 :
                                                  0;
            }
        }

        private static int[][] ParseInput(string[] input)
        {
            var regex  = new Regex(@"<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>");
            var output = new int[4][];

            for (int i = 0; i < 4; i++)
            {
                var m = regex.Match(input[i]);
                output[i] = new int[3];

                output[i][0] = int.Parse(m.Groups["x"].Value);
                output[i][1] = int.Parse(m.Groups["y"].Value);
                output[i][2] = int.Parse(m.Groups["z"].Value);
            }

            return output;
        }
    }
}

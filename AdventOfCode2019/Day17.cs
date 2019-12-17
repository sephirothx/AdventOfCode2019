using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ReStructure.Sparse;

namespace AdventOfCode2019
{
    class Day17
    {
        private static readonly SparseMatrix<char> map = new SparseMatrix<char>('.');
        private static          (int x, int y)     pos;
        private static          (int x, int y)     dir;

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            long ip = 0;
            int x = 0,
                y = 0;

            int ans = 0;

            while (true)
            {
                intcode.Compute(program, ip, false);
                if (intcode.IsOver)
                {
                    break;
                }

                if (intcode.Output == 10)
                {
                    y++;
                    x = 0;
                }
                else
                {
                    char c   = (char)intcode.Output;
                    int  idx = "v^><".IndexOf(c) + 1;

                    if (idx != 0)
                    {
                        pos = (x, y);
                        dir = Utility.GetDirection((Direction)idx);
                    }

                    map[x++, y] = c;
                }

                program = intcode.State;
                ip      = intcode.IP;
            }

            for (x = 0; x <= map.MaxX; x++)
            {
                for (y = 0; y <= map.MaxY; y++)
                {
                    if (map[x, y] == '.')
                        continue;

                    bool test = true;
                    foreach ((int nx, int ny) in Neighbors(x, y))
                    {
                        test &= map[nx, ny] != '.';
                    }

                    ans += test ? x * y : 0;
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            program[0] = 2;

            string fullPath = GetFullPath();
            string routine  = GetRoutine(fullPath);

            foreach (char c in routine)
            {
                intcode.AddInput(c);
            }

            intcode.Compute(program);
            Console.WriteLine(intcode.Output);
        }

        private static string GetRoutine(string fullPath, char videoFeed = 'n')
        {
            var regex = new Regex(@"^(.{1,21})\1*(.{1,21})(?:\1|\2)*(.{1,21})(?:\1|\2|\3)*$");
            var match = regex.Match(fullPath);

            string f1 = match.Groups[1].Value.TrimEnd(',');
            string f2 = match.Groups[2].Value.TrimEnd(',');
            string f3 = match.Groups[3].Value.TrimEnd(',');

            string main = fullPath.Replace(f1, "A")
                                  .Replace(f2, "B")
                                  .Replace(f3, "C")
                                  .TrimEnd(',');

            return $"{main}\n{f1}\n{f2}\n{f3}\n{videoFeed}\n";
        }

        private static string GetFullPath()
        {
            var path  = new StringBuilder();
            int count = 0;

            while (true)
            {
                (int x, int y) next = Utility.TupleSum(pos, dir);

                if (map[next.x, next.y] == '#')
                {
                    count++;
                    pos = next;
                }
                else
                {
                    if (count > 0)
                    {
                        path.Append(count.ToString() + ',');
                        count = 0;
                    }

                    var left = Utility.RotateLeft(dir);
                    next = Utility.TupleSum(pos, left);
                    if (map[next.x, next.y] == '#')
                    {
                        dir = left;
                        path.Append("L,");
                        continue;
                    }

                    var right = Utility.RotateRight(dir);
                    next = Utility.TupleSum(pos, right);
                    if (map[next.x, next.y] == '#')
                    {
                        dir = right;
                        path.Append("R,");
                        continue;
                    }

                    break;
                }
            }

            return path.ToString();
        }

        public static void Part2_manual(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();

            program[0] = 2;

            const string INPUT = "A,B,A,C,B,C,A,B,A,C\n" + // Main:
                                 "R,6,L,10,R,8,R,8\n"    + // Function A:
                                 "R,12,L,8,L,10\n"       + // Function B:
                                 "R,12,L,10,R,6,L,10\n"  + // Function C:
                                 "n\n";                    // Continuous video feed?

            foreach (char c in INPUT)
            {
                intcode.AddInput(c);
            }

            intcode.Compute(program);
            Console.WriteLine(intcode.Output);
        }

        private static List<(int x, int y)> Neighbors(int x, int y)
        {
            var output = new List<(int x, int y)>();
            for (int i = 1; i < 5; i++)
            {
                var d = Utility.GetDirection((Direction)i);
                output.Add((x + d.x, y + d.y));
            }

            return output;
        }
    }
}

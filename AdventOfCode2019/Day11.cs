using System;
using ReStructure.Sparse;

namespace AdventOfCode2019
{
    class Day11
    {
        private const int BLACK = 0;
        private const int WHITE = 1;

        private class Robot
        {
            private readonly SparseMatrix<int> _map;

            private int _step;
            private (int x, int y) _pos = (0, 0);
            private (int x, int y) _dir = (0, -1);

            public int minx, miny, maxx, maxy;

            public Robot(int i)
            {
                _map = new SparseMatrix<int>(i);
            }

            public void SetInput(int input)
            {
                if (_step++ % 2 == 0)
                {
                    _map[_pos.x,_pos.y] = input;
                }
                else
                {
                    int tmp = _dir.x;
                    _dir.x = _dir.y * (input == 0 ? 1 : -1);
                    _dir.y = tmp * (input == 0 ? -1 : 1);

                    _pos.x += _dir.x;
                    _pos.y += _dir.y;

                    minx = Math.Min(minx, _pos.x);
                    miny = Math.Min(miny, _pos.y);
                    maxx = Math.Max(maxx, _pos.x);
                    maxy = Math.Max(maxy, _pos.y);
                }
            }

            public int GetOutput()
            {
                return _map[_pos.x, _pos.y];
            }

            public int GetResult()
            {
                return _map.Count;
            }

            public SparseMatrix<int> GetMap()
            {
                return _map;
            }
        }

        public static void Part1(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();
            var robot   = new Robot(BLACK);

            Run(program, intcode, robot);

            Console.WriteLine(robot.GetResult());
        }

        public static void Part2(string input)
        {
            var program = Intcode.ParseInput(input);
            var intcode = new Intcode();
            var robot   = new Robot(WHITE);

            Run(program, intcode, robot);

            var map = robot.GetMap();
            for (int i = robot.miny; i <= robot.maxy; i++)
            {
                for (int j = robot.minx; j <= robot.maxx; j++)
                {
                    Console.Write(map[j, i] == 0 ? ' ' : '#');
                }

                Console.WriteLine();
            }
        }

        private static void Run(long[] program, Intcode intcode, Robot robot)
        {
            long ip = 0;

            while (intcode.IsOver == false)
            {
                intcode.Input = robot.GetOutput();
                intcode.Compute(program, ip, false);
                robot.SetInput((int)intcode.Output);
                program = intcode.State;
                ip = intcode.IP;
                intcode.Compute(program, ip, false);
                robot.SetInput((int)intcode.Output);
                program = intcode.State;
                ip = intcode.IP;
            }
        }
    }
}
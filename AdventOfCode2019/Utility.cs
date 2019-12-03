using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public static class Utility
    {
        #region ManhattanDistance

        public static int ManhattanDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }

        public static int ManhattanDistance((int x, int y) p1, (int x, int y) p2)
            => ManhattanDistance(p1.x, p1.y, p2.x, p2.y);

        public static int ManhattanDistance(int x, int y)
            => ManhattanDistance(x, y, 0, 0);

        public static int ManhattanDistance((int x, int y) p)
            => ManhattanDistance(p.x, p.y);

        #endregion
    }
}

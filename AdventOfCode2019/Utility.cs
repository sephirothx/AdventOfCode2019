using System;

namespace AdventOfCode2019
{
    public enum Direction
    {
        Right,
        Left,
        Down,
        Up
    }

    public static class Utility
    {
        public static (int x, int y) GetDirection(Direction dir)
        {
            switch (dir)
            {
            case Direction.Right: return (1, 0);
            case Direction.Left:  return (-1, 0);
            case Direction.Down:  return (0, -1);
            case Direction.Up:    return (0, 1);
            default:              throw new ArgumentOutOfRangeException(nameof(dir), dir, null);
            }
        }

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

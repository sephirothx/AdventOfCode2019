using System;
using System.IO;
using System.Net;

namespace AdventOfCode2019
{
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Right = 3,
        Left = 4
    }

    public static class Utility
    {
        public const string INPUT_PATH = @"input.txt";
        public const string COOKIE_PATH = @"cookie.txt";

        #region Direction

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

        public static (int x, int y) RotateLeft((int x, int y) dir)
        {
            int tmp = dir.x;
            dir.x = dir.y;
            dir.y = -tmp;

            return dir;
        }

        public static (int x, int y) RotateRight((int x, int y) dir)
        {
            int tmp = dir.x;
            dir.x = -dir.y;
            dir.y = tmp;

            return dir;
        }

        public static (int x, int y) TupleSum((int x, int y) p1, (int x, int y) p2)
        {
            return (p1.x + p2.x, p1.y + p2.y);
        }

        #endregion

        #region GCD_LCM

        public static int GCD(int a, int b)
            => (int)GCD((long)a, b);

        public static int LCM(int a, int b)
        {
            return a / GCD(a, b) * b;
        }

        public static long GCD(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            long gcd = a == 0 ? b : a;

            return gcd;
        }

        public static long LCM(long a, long b)
        {
            return a / GCD(a, b) * b;
        }

        #endregion

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

        public static void FetchInput()
        {
            int day  = DateTime.Today.Day;
            int year = DateTime.Today.Year;

            string INPUT_URL = @$"https://adventofcode.com/{year}/day/{day}/input";

            var req = (HttpWebRequest)WebRequest.Create(INPUT_URL);
            var cookie = new Cookie("session",
                                    File.ReadAllText(COOKIE_PATH),
                                    "/", ".adventofcode.com");
            (req.CookieContainer ??= new CookieContainer()).Add(cookie);

            var response = req.GetResponse().GetResponseStream();
            if (response == null) return;

            using var reader = new StreamReader(response);

            string input = reader.ReadToEnd();
            File.WriteAllText(INPUT_PATH, input.TrimEnd('\n'));
        }
    }
}

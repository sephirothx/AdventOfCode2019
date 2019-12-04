using System;

namespace AdventOfCode2019
{
    class Day4
    {
        private const int MIN = 402328;
        private const int MAX = 864247;

        public static void Part1(string[] input)
        {
            int ans = 0;
            
            for (int i = MIN; i < MAX; i++)
            {
                int prev = 0;

                bool rule1 = false;
                bool rule2 = true;

                for (int j = 0; j < 6; j++)
                {
                    int curr = i / (int)Math.Pow(10, 5 - j) % 10;

                    if (curr == prev)
                    {
                        rule1 = true;
                    }

                    if (curr < prev)
                    {
                        rule2 = false;
                    }

                    prev = curr;
                }

                if (rule1 && rule2)
                {
                    ans++;
                }
            }

            Console.WriteLine(ans);
        }

        public static void Part2(string[] input)
        {
            int ans = 0;
            
            for (int i = MIN; i < MAX; i++)
            {
                int prev = 0;

                bool rule1 = false;
                bool rule2 = true;

                var digit = new int[10];

                for (int j = 0; j < 6; j++)
                {
                    int curr = i / (int)Math.Pow(10, 5 - j) % 10;

                    if (curr < prev)
                    {
                        rule2 = false;
                    }

                    digit[curr]++;

                    prev = curr;
                }

                for (int j = 0; j < 10; j++)
                {
                    if (digit[j] == 2)
                    {
                        rule1 = true;
                    }
                }

                if (rule1 && rule2)
                {
                    ans++;
                }
            }

            Console.WriteLine(ans);
        }
    }
}
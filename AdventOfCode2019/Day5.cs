using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2019
{
    class Day5
    {
        public static void Part1(string input)
        {
            var program = ParseInput(input);

            Intcode.Instance.Compute(program, 1);
        }

        public static void Part2(string input)
        {
            var program = ParseInput(input);

            Intcode.Instance.Compute(program, 5);
        }

        private static int[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(int.Parse)
                        .ToArray();
        }

        private static void Compute(int[] program, int input)
        {
            int ip = 0;

            while (true)
            {
                int opcode = program[ip] % 100;

                int mode1 = program[ip] / 100 % 10;
                int mode2 = program[ip] / 1000 % 10;
                //int mode3 = program[i] / 10000 % 10;

                switch (opcode)
                {
                case 1:
                    int num1   = program[ip + 1];
                    int num2   = program[ip + 2];
                    int target = program[ip + 3];
                    
                    int op1 = mode1 == 1 ? num1 : program[num1];
                    int op2 = mode2 == 1 ? num2 : program[num2];

                    program[target] = op1 + op2;
                    ip += 4;
                    break;
                case 2:
                    num1   = program[ip + 1];
                    num2   = program[ip + 2];
                    target = program[ip + 3];

                    op1 = mode1 == 1 ? num1 : program[num1];
                    op2 = mode2 == 1 ? num2 : program[num2];

                    program[target] = op1 * op2;
                    ip += 4;
                    break;
                case 3:
                    num1 = program[ip + 1];
                    program[num1] = input;
                    ip += 2;
                    break;
                case 4:
                    num1 = program[ip + 1];
                    Console.WriteLine(mode1 == 1 ? num1 : program[num1]);
                    ip += 2;
                    break;
                case 5:
                    num1 = program[ip + 1];
                    num2 = program[ip + 2];

                    op1 = mode1 == 1 ? num1 : program[num1];
                    op2 = mode2 == 1 ? num2 : program[num2];

                    ip = op1 != 0 ? op2 : ip + 3;
                    break;
                case 6:
                    num1 = program[ip + 1];
                    num2 = program[ip + 2];

                    op1 = mode1 == 1 ? num1 : program[num1];
                    op2 = mode2 == 1 ? num2 : program[num2];

                    ip = op1 == 0 ? op2 : ip + 3;
                    break;
                case 7:
                    num1   = program[ip + 1];
                    num2   = program[ip + 2];
                    target = program[ip + 3];

                    op1 = mode1 == 1 ? num1 : program[num1];
                    op2 = mode2 == 1 ? num2 : program[num2];

                    program[target] = op1 < op2 ? 1 : 0;
                    ip += 4;
                    break;
                case 8:
                    num1   = program[ip + 1];
                    num2   = program[ip + 2];
                    target = program[ip + 3];

                    op1 = mode1 == 1 ? num1 : program[num1];
                    op2 = mode2 == 1 ? num2 : program[num2];

                    program[target] = op1 == op2 ? 1 : 0;
                    ip += 4;
                    break;
                case 99:
                    return;
                }
            }
        }
    }
}
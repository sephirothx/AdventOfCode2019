using System;
using System.Collections.Generic;

namespace AdventOfCode2019
{
    public class Intcode
    {
        #region Private

        private enum Opcode
        {
            ADD = 1,
            MUL = 2,
            IN  = 3,
            OUT = 4,
            JT  = 5,
            JF  = 6,
            LES = 7,
            EQ  = 8,

            HALT = 99
        }

        private enum Mode
        {
            Parameter = 0,
            Immediate = 1
        }

        private enum Type
        {
            READ,
            WRITE
        }

        private static readonly Dictionary<Opcode, Type[]> _argTypes = new Dictionary<Opcode, Type[]>
        {
            {Opcode.ADD, new[] {Type.READ, Type.READ, Type.WRITE}},
            {Opcode.MUL, new[] {Type.READ, Type.READ, Type.WRITE}},
            {Opcode.IN, new[] {Type.WRITE}},
            {Opcode.OUT, new[] {Type.READ}},
            {Opcode.JT, new[] {Type.READ, Type.READ}},
            {Opcode.JF, new[] {Type.READ, Type.READ}},
            {Opcode.LES, new[] {Type.READ, Type.READ, Type.WRITE}},
            {Opcode.EQ, new[] {Type.READ, Type.READ, Type.WRITE}},
            {Opcode.HALT, new Type[0]}
        };

        #endregion

        private static Intcode _instance;
        public static  Intcode Instance => _instance ??= new Intcode();

        private int   _ip;
        private int[] _program;

        private Intcode()
        {
        }

        public void Compute(int[] program, int input = 0)
        {
            _program = program;

            _ip = 0;

            while (true)
            {
                var (opcode, modes) = Prefetch();
                var args = Fetch(opcode, modes);

                switch (opcode)
                {
                case Opcode.ADD:
                    _program[args[2]] = args[0] + args[1];
                    break;

                case Opcode.MUL:
                    _program[args[2]] = args[0] * args[1];
                    break;

                case Opcode.IN:
                    _program[args[0]] = input;
                    break;

                case Opcode.OUT:
                    Console.WriteLine(args[0]);
                    break;

                case Opcode.JT:
                    if (args[0] != 0) _ip = args[1];
                    break;

                case Opcode.JF:
                    if (args[0] == 0) _ip = args[1];
                    break;

                case Opcode.LES:
                    _program[args[2]] = args[0] < args[1] ? 1 : 0;
                    break;

                case Opcode.EQ:
                    _program[args[2]] = args[0] == args[1] ? 1 : 0;
                    break;

                case Opcode.HALT:
                    return;

                default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private int[] Fetch(Opcode opcode, Mode[] modes)
        {
            var argTypes = _argTypes[opcode];
            int argc     = argTypes.Length;

            var args = new int[argc];
            for (int i = 0; i < argc; i++)
            {
                args[i] = GetArg(argTypes[i], modes[i]);
            }

            return args;
        }

        private int GetArg(Type type, Mode mode)
        {
            int value = _program[_ip++];

            int output = (type, mode) switch
            {
                (Type.READ, Mode.Parameter) => _program[value],
                _                           => value
            };

            return output;
        }

        private (Opcode opcode, Mode[] modes) Prefetch()
        {
            int instr  = _program[_ip++];
            var opcode = (Opcode)(instr % 100);

            int argc = _argTypes[opcode].Length;
            var mode = new Mode[argc];

            for (int i = 0; i < argc; i++)
            {
                mode[i] = (Mode)(instr / (int)Math.Pow(10, i + 2) % 10);
            }

            return (opcode, mode);
        }
    }
}

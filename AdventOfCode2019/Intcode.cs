using System;
using System.Collections.Generic;
using System.Linq;

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
            REL = 9,

            HALT = 99
        }

        private enum Mode
        {
            Parameter = 0,
            Immediate = 1,
            Relative  = 2
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
            {Opcode.REL, new[] {Type.READ}},
            {Opcode.HALT, new Type[0]}
        };

        #endregion

        private static Intcode _instance;
        public static  Intcode Instance => _instance ??= new Intcode();

        private readonly Queue<long> _inputs = new Queue<long>();

        public long Input
        {
            set => _inputs.Enqueue(value);
        }

        public long Output { get; private set; }

        public long[] State { get; private set; }
        public long   IP    { get; private set; }

        public bool IsOver { get; private set; }

        private long _base;

        public static long[] ParseInput(string input)
        {
            return input.Split(',')
                        .Select(long.Parse)
                        .ToArray();
        }

        public void Compute(long[] program, long ip = 0, bool runToEnd = true)
        {
            State = new long[10000];
            Array.Copy(program, State, program.Length);

            IP = ip;

            while (true)
            {
                var (opcode, modes) = Prefetch();
                var args = Fetch(opcode, modes);

                switch (opcode)
                {
                case Opcode.ADD:
                    State[args[2]] = args[0] + args[1];
                    break;

                case Opcode.MUL:
                    State[args[2]] = args[0] * args[1];
                    break;

                case Opcode.IN:
                    State[args[0]] = _inputs.Dequeue();
                    // Day13 - part 2 interactive
                    // var key = Console.ReadKey().Key;
                    // State[args[0]] = key == ConsoleKey.LeftArrow  ? -1 :
                    //                  key == ConsoleKey.RightArrow ? 1 : 0;
                    break;

                case Opcode.OUT:
                    Output = args[0];
                    IsOver = false;
                    //Console.WriteLine(Output);
                    if (runToEnd == false) return;
                    break;

                case Opcode.JT:
                    if (args[0] != 0) IP = args[1];
                    break;

                case Opcode.JF:
                    if (args[0] == 0) IP = args[1];
                    break;

                case Opcode.LES:
                    State[args[2]] = args[0] < args[1] ? 1 : 0;
                    break;

                case Opcode.EQ:
                    State[args[2]] = args[0] == args[1] ? 1 : 0;
                    break;

                case Opcode.REL:
                    _base += args[0];
                    break;

                case Opcode.HALT:
                    IsOver = true;
                    return;

                default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private long[] Fetch(Opcode opcode, Mode[] modes)
        {
            var argTypes = _argTypes[opcode];
            int argc     = argTypes.Length;

            var args = new long[argc];
            for (int i = 0; i < argc; i++)
            {
                args[i] = GetArg(argTypes[i], modes[i]);
            }

            return args;
        }

        private long GetArg(Type type, Mode mode)
        {
            long value = State[IP++];

            long output = (type, mode) switch
            {
                (Type.READ, Mode.Parameter) => State[value],
                (Type.READ, Mode.Relative)  => State[value + _base],
                (Type.WRITE, Mode.Relative) => value + _base,
                _                           => value
            };

            return output;
        }

        private (Opcode opcode, Mode[] modes) Prefetch()
        {
            long instr  = State[IP++];
            var  opcode = (Opcode)(instr % 100);

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

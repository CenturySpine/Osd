using System;

namespace O.S.D.DungeonGenerator.DungeonGeneration
{
    internal static class NumGen
    {
        private static Random Generator { get; }

        static NumGen()
        {
            Generator = new Random((int)(DateTime.Now.Millisecond / 0.9));
        }

        public static int Next(int min, int max)
        {
            return Generator.Next(min, max);
        }
    }
}
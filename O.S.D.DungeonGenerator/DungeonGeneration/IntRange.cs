namespace O.S.D.DungeonGenerator.DungeonGeneration
{
    class IntRange
    {


        public IntRange(int v1, int v2)
        {
            Min = v1;
            Max = v2;
        }

        public int Min { get;  }
        public int Max { get;  }

        
        internal int Next()
        {
            return NumGen.Next(Min, Max + 1);
        }
    }
}
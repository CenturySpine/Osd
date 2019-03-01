namespace O.S.D.DungeonGenerator.DungeonGeneration
{
    internal class DungeonParameters
    {
        public DungeonParameters()
        {
            PixelWidth = 100;
            PixelHeight = 100;
            TileSize = 5;
            AreaWidth = PixelWidth / TileSize;
            AreaHeight = PixelHeight / TileSize;
            RoomNumber = new IntRange(5,25);
            RoomHeight = new IntRange(3, 10);
            RoomWidth = new IntRange(3, 10);

            CorridorLength = new IntRange(2,10);

            CorridorWidth = new IntRange(1,1);

            OverlapTolerance = 5;

        }

        public int OverlapTolerance { get; set; }

        public IntRange CorridorWidth { get; set; }

        public IntRange CorridorLength { get; set; }

        public IntRange RoomWidth { get; set; }

        public IntRange RoomHeight { get; set; }

        public int AreaHeight { get; set; }

        public IntRange RoomNumber { get; set; }

        public int AreaWidth { get; set; }
        public int TileSize { get; set; }
        public int PixelWidth { get; }
        public int PixelHeight { get; }
    }
}
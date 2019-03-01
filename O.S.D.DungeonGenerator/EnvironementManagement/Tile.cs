namespace O.S.D.DungeonGenerator.EnvironementManagement
{
    internal class Tile
    {
        public TileType TileType { get; set; }

        public override string ToString()
        {
            return $"{nameof(TileType)}: {TileType}";
        }
    }
}
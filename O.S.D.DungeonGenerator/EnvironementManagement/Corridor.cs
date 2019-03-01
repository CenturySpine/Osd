namespace O.S.D.DungeonGenerator.EnvironementManagement
{
    internal class Corridor : DeplacementArea
    {
        public Direction Direction { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Direction)}: {Direction}";
        }
    }
}
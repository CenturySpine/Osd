namespace O.S.D.DungeonGenerator.EnvironementManagement
{
    internal class DeplacementArea
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Xpos { get; set; }
        public int YPos { get; set; }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}, {nameof(Xpos)}: {Xpos}, {nameof(YPos)}: {YPos}";
        }
    }
}
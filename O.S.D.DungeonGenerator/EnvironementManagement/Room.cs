using O.S.D.DungeonGenerator.DungeonGeneration;
using System;

namespace O.S.D.DungeonGenerator.EnvironementManagement
{
    internal class Room : DeplacementArea
    {
        public Room()
        {
        }

        public int ExitXPos { get; set; }
        public int ExitYPos { get; set; }

        public void SetExitDir(Direction getExitDirection)
        {
            switch (getExitDirection)
            {
                case Direction.North:
                    ExitXPos = NumGen.Next(Xpos, Xpos + Width);
                    ExitYPos = YPos;

                    break;

                case Direction.East:
                    ExitXPos = Xpos + Width - 1;
                    ExitYPos = NumGen.Next(YPos, YPos + Height);

                    break;

                case Direction.South:
                    ExitXPos = NumGen.Next(Xpos, Xpos + Width);
                    ExitYPos = YPos + Height - 1;

                    break;

                case Direction.West:
                    ExitXPos = Xpos;
                    ExitYPos = NumGen.Next(YPos, YPos + Height); ;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(getExitDirection), getExitDirection, null);
            }

            ExitDirection = getExitDirection;
        }

        public Direction ExitDirection { get; set; }
        public int Entrance { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()},  {nameof(Entrance)}: {Entrance}, {nameof(ExitXPos)}: {ExitXPos}, {nameof(ExitYPos)}: {ExitYPos}, {nameof(ExitDirection)}: {ExitDirection}";
        }
    }
}
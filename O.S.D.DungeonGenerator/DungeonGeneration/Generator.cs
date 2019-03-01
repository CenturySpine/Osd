using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.EnvironementManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace O.S.D.DungeonGenerator.DungeonGeneration
{

    enum StartingPosition
    {
        LowerLeft,
        LowerRight,
        UpperRight,
        UpperLeft,
        Center
    }
    internal class Generator
    {
        //private int corridorTries = 0;
        //private int roomTries = 0;
        private DungeonParameters _dp;

        private int maxtriesCorridor = 100;

        private int maxtriesRoom = 100;
        private StartingPosition _startingpos;

        public Generator(DungeonParameters dp)
        {
            _dp = dp;
            Logs = new ObservableCollection<string>();
            Clear();
        }

        public List<Corridor> Corridors { get; set; }
        public ObservableCollection<string> Logs { get; set; }

        public List<Room> Rooms { get; set; }

        public Tile[,] TilesMatrix { get; set; }

        public bool CanMove(int playerXPos, int playerYPos)
        {
            if (playerXPos >= TilesMatrix.GetLength(0) || playerYPos >= TilesMatrix.GetLength(1))
            {
                return false;
            }
            var tile = TilesMatrix[playerXPos, playerYPos];
            return tile.TileType != TileType.None && tile.TileType != TileType.Pilar;
        }

        public void Clear()
        {
            currentoverlap = 0;
               TilesMatrix = new Tile[_dp.AreaWidth, _dp.AreaHeight];
            for (int i = 0; i < _dp.AreaWidth; i++)
            {
                for (int j = 0; j < _dp.AreaHeight; j++)
                {
                    TilesMatrix[i, j] = new Tile();
                }
            }
            Logs.Clear();
            Rooms = new List<Room>();
            Corridors = new List<Corridor>();
        }

        public async Task Genrate()
        {
            _startingpos = (StartingPosition)NumGen.Next(0, 5) /*StartingPosition.Center*/;
            LogToUiThread($"Starting position : {_startingpos}");

            int roomcount = _dp.RoomNumber.Next();
            LogToUiThread($"Generating {roomcount} rooms");
            for (int i = 0; i < roomcount; i++)
            {

                bool isFirst = i == 0;
                Direction dir = Direction.North;
                if (isFirst)
                {
                    dir = GetDirByStartupPos();
                }
                else
                {
                    dir = (Direction)NumGen.Next(0, 4);
                }


                var r = await GenerateRoom(dir, (Corridors.Count > 0) ? Corridors[i - 1] : null, (isFirst));
                Rooms.Add(r);

                //si dernière salle, pas couloir
                if (i < roomcount - 1)
                {
                    var c = await GenerateCorridor(r);
                    Corridors.Add(c);
                }
            }
        }

        private Direction GetDirByStartupPos()
        {
            switch (_startingpos)
            {
                case StartingPosition.LowerLeft:
                    return (Direction)NumGen.Next(0, 2);
                case StartingPosition.UpperLeft:
                    return (Direction)NumGen.Next(1, 3);
                case StartingPosition.UpperRight:
                    return (Direction)NumGen.Next(2, 4);
                case StartingPosition.LowerRight:
                    int[] dirs = new int[2] { (int)Direction.North, (int)Direction.West };
                    return (Direction)dirs[NumGen.Next(0, dirs.Length)];
                case StartingPosition.Center:
                    return (Direction)NumGen.Next(0, 4);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void PlacePlayerStartPosition(Player player)
        {
            var initialRoom = Rooms[0];

            player.XPos = initialRoom.Xpos;
            player.YPos = initialRoom.YPos;
        }

        private bool CheckArea(DeplacementArea da)
        {
            if (da.Xpos + da.Width > _dp.AreaWidth || da.Xpos < 0 || da.YPos < 0 || da.YPos + da.Height > _dp.AreaHeight)
            {
                return true;
            }

            for (int i = da.Xpos; i < da.Width + da.Xpos; i++)
            {
                for (int j = da.YPos; j < da.Height + da.YPos; j++)
                {
                    if (TilesMatrix[i, j].TileType != TileType.None)
                    {
                        currentoverlap++;
                        if (currentoverlap < _dp.OverlapTolerance)
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private int currentoverlap = 0;
        private async Task<Corridor> GenerateCorridor(Room room)
        {
            Corridor c = await VerifyCorridor(room);

            for (int i = c.Xpos; i < c.Width + c.Xpos; i++)
            {
                for (int j = c.YPos; j < c.Height + c.YPos; j++)
                {
                    TilesMatrix[i, j] = new Tile { TileType = TileType.Corridor };
                }
            }
            LogToUiThread($"{nameof(Corridor)} : {c.ToString()}");

            return c;
        }

        private async Task<Room> GenerateRoom(Direction dir, Corridor corridor, bool isFirst)
        {
            //if (corridor == null)
            //{
            var room = await VerifyRoom(isFirst, corridor);

            room.SetExitDir(dir);

            for (int i = room.Xpos; i < room.Width + room.Xpos; i++)
            {
                for (int j = room.YPos; j < room.Height + room.YPos; j++)
                {
                    if (i == room.ExitXPos && j == room.ExitYPos)
                    {
                        TilesMatrix[i, j] = new Tile { TileType = TileType.ExitEntrance };
                    }
                    else
                    {
                        TilesMatrix[i, j] = new Tile { TileType = TileType.Room };
                    }
                }
            }
            LogToUiThread($"{nameof(Room)} : {room.ToString()}");

            return room;
            //}
        }

        private Direction GetExitDirection()
        {
            return (Direction)NumGen.Next(0, 4);
        }

        private int GetRoomHeight()
        {
            return _dp.RoomHeight.Next();
        }

        private int GetRoomWidth()
        {
            return _dp.RoomWidth.Next();
        }

        private void LogToUiThread(string s)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                Logs.Add(s);
            }));
        }

        private async Task<Corridor> VerifyCorridor(Room room)
        {
            return await Task.Run(() =>
                {
                    bool retry = true;
                    Corridor c = null;
                    int tries = 0;
                    while (retry)
                    {
                        c = new Corridor();
                        var dir = room.ExitDirection;
                        c.Direction = dir;
                        switch (dir)
                        {
                            case Direction.North:
                                c.Width = NumGen.Next(1, 2);
                                c.Height = _dp.CorridorLength.Next();
                                c.Xpos = room.ExitXPos;
                                c.YPos = room.ExitYPos - c.Height;
                                //c.Height = Helpers.Clamp(c.Height, 1, _dp.AreaHeight - 1);
                                break;

                            case Direction.East:
                                c.Xpos = room.ExitXPos + 1;
                                c.YPos = room.ExitYPos;
                                c.Height = NumGen.Next(1, 2);
                                c.Width = _dp.CorridorLength.Next();
                                //c.Width = Helpers.Clamp(c.Width, 1, _dp.AreaWidth - 1);
                                break;

                            case Direction.South:
                                c.Xpos = room.ExitXPos;
                                c.YPos = room.ExitYPos + 1;
                                c.Width = NumGen.Next(1, 2);
                                c.Height = _dp.CorridorLength.Next();
                                //c.Height = Helpers.Clamp(c.Height, 1, _dp.AreaHeight - 1);
                                break;

                            case Direction.West:
                                c.Height = NumGen.Next(1, 2);
                                c.Width = _dp.CorridorLength.Next();
                                c.Xpos = room.ExitXPos - c.Width;
                                c.YPos = room.ExitYPos;
                                //c.Width = Helpers.Clamp(c.Width, 1, _dp.AreaWidth - 1);

                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        retry = CheckArea(c);
                        tries++;
                        if (tries >= maxtriesCorridor)
                        {
                            LogToUiThread($"Verify corridor tries : {tries}");

                            throw new GenerationabortedException("Generation canceled du to too many tries");
                        }
                        Thread.Sleep(10);
                    }

                    LogToUiThread($"Verify corridor tries : {tries}");

                    return c;
                })
                ;
        }

        private async Task<Room> VerifyRoom(bool isfirst, Corridor corridor)
        {
            return await Task.Run(() =>
            {
                Room room = null;
                bool retry = true;
                int tries = 0;
                while (retry)
                {
                    room = new Room
                    {
                        Height = GetRoomHeight(),
                        Width = GetRoomWidth(),
                    };
                    if (isfirst)
                    {
                        switch (_startingpos)
                        {
                            case StartingPosition.LowerLeft:
                                room.Xpos = 1;
                                room.YPos = _dp.AreaHeight - 1 - room.Height;
                                break;
                            case StartingPosition.LowerRight:
                                room.Xpos = _dp.AreaWidth - 1 - room.Width;
                                room.YPos = _dp.AreaHeight - 1 - room.Height;
                                break;
                            case StartingPosition.UpperRight:
                                room.Xpos = _dp.AreaWidth - 1 - room.Width;
                                room.YPos = 1;
                                break;
                            case StartingPosition.UpperLeft:
                                room.Xpos = 1;
                                room.YPos = 1;
                                break;
                            case StartingPosition.Center:
                                room.Xpos = _dp.AreaWidth / 2 - room.Width / 2;
                                room.YPos = _dp.AreaHeight / 2 - room.Height / 2;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                    }
                    else if (corridor != null)
                    {
                        int entrance;
                        switch (corridor.Direction)
                        {
                            case Direction.North:
                                entrance = NumGen.Next(0, room.Width);
                                room.YPos = corridor.YPos - room.Height;

                                room.Xpos = corridor.Xpos - entrance;
                                break;

                            case Direction.East:
                                entrance = NumGen.Next(0, room.Height);

                                room.Xpos = corridor.Xpos + corridor.Width;
                                room.YPos = corridor.YPos - entrance;
                                break;

                            case Direction.South:
                                entrance = NumGen.Next(0, room.Width);

                                room.YPos = corridor.YPos + corridor.Height;

                                room.Xpos = corridor.Xpos - entrance;
                                break;

                            case Direction.West:
                                entrance = NumGen.Next(0, room.Height);
                                room.Xpos = corridor.Xpos - room.Width;

                                room.YPos = corridor.YPos - entrance;
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        room.Entrance = entrance;
                    }

                    retry = CheckArea(room);
                    tries++;
                    if (tries >= maxtriesRoom)
                    {
                        LogToUiThread($"Verify Room tries : {tries}");

                        throw new GenerationabortedException("Generation canceled du to too many tries");
                    }
                    Thread.Sleep(10);
                }

                LogToUiThread($"Verify Room tries : {tries}");

                return room;
            });
        }

        internal class GenerationabortedException : Exception
        {
            public GenerationabortedException(string message) : base(message)
            {
            }
        }
    }
}
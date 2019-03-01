using O.S.D.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace O.S.D.GameManagement
{
    internal class TileDiscoveringService : ITileDiscoveringService
    {
        private readonly IAreaInfos _areainfos;
        private Random _cardinalTileGen;
        private Tile[,] _ccordarray;
        private List<Tile> _ccords;
        private Random _corridorBalance;
        private Random _deadEndRestart;
        private Random _dispersionBalance;
        private List<CardinalTileOrientation> _occupiedDirections;
        private Tile _previous;
        private int _tries = 0;
        public TileDiscoveringService(IAreaInfos areainfos)
        {
            _areainfos = areainfos;


            _ccords = new List<Tile>();
            CreateRandomizers();
            _ccordarray = new Tile[_areainfos.AreaWidth, _areainfos.AreaHeight];
            TileCreationLogs = new ObservableCollection<string>();
            _occupiedDirections = new List<CardinalTileOrientation>();
        }

        public ObservableCollection<string> TileCreationLogs { get; }

        public void Clear()
        {
            _tries = 0;
            _ccords.Clear();
            TileCreationLogs.Clear();
            _ccordarray = new Tile[_areainfos.AreaWidth, _areainfos.AreaHeight];
            _occupiedDirections.Clear();
            //CreateRandomizers();
            _areainfos.ClearOccupations();

            _previous = null;
        }
        public Tile New(bool mustRestart, CardinalTileOrientation? force)
        {
            //_cardinalTileGen = new Random(DateTime.Now.Millisecond);

            if (_tries >= _areainfos.MaxtriesToReachNewOccupationArea)
            {
                _tries = 0;
                TileCreationLogs.Insert(0, $"Generation Ended");
                return null;
            }
            if (mustRestart)
            {
                //var third = _ccords.Count / 3;

                var potentialfreetiles = _ccords.Where(c => ReturnFreeSides(c) >= 2).ToList();
                var newStartTileIndex = _deadEndRestart.Next(0, potentialfreetiles.Count);

                if (potentialfreetiles.Any())
                {
                    _previous = potentialfreetiles[newStartTileIndex];
                    TileCreationLogs.Insert(0, $"Restarting at : x={_previous.X}, y={_previous.Y}");
                }
                else
                {
                    //_mustREstart = false;

                    _tries = 0;
                    TileCreationLogs.Insert(0, $"Generation Ended");
                    return null;
                }
            }
            if (_previous == null)
            {
                var first = new Tile { X = _areainfos.StartX, Y = _areainfos.StartY, Index = 0 };
                _previous = first;
                CalculatePlacement(first);
                ComputeAreaOccupation(GetOccupationArea(first), first);

                AddTile(first, null);
                //TileCreationLogs.Insert(0, $"First tile creation : x={first.X}, y={first.Y}");
                return first;
            }

            Tile next = new Tile();
            var or = GetOrientation(force);

            switch (or)
            {
                case CardinalTileOrientation.Est:
                    next.X = _previous.X + 1;
                    next.Y = _previous.Y;
                    break;

                case CardinalTileOrientation.South:
                    next.X = _previous.X;
                    next.Y = _previous.Y + 1;
                    break;

                case CardinalTileOrientation.West:
                    next.X = _previous.X - 1;
                    next.Y = _previous.Y;
                    break;

                case CardinalTileOrientation.North:
                    next.X = _previous.X;
                    next.Y = _previous.Y - 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            CalculatePlacement(next);

            OccupationArea ocar = GetOccupationArea(next);
            CardinalTileOrientation? orientationForce = null;
            var checkforoccupation = force.HasValue ? false : IsOccupationThresoldReached(next, ocar, ref orientationForce);
            if (IsCoordOccupiedOrOut(next) || MustDisperse(next) || MustBeCorridor(next) || checkforoccupation /*false*/)
            {
                bool mustREstart = false;
                if (!_occupiedDirections.Contains(or))
                {
                    _occupiedDirections.Add(or);
                }
                if (_occupiedDirections.Count == 4)
                {
                    TileCreationLogs.Insert(0, $"Dead End !");
                    _occupiedDirections.Clear();
                    mustREstart = true;
                }
                _tries++;
                return New(mustREstart, orientationForce);
            }
            _tries = 0;
            ComputeAreaOccupation(ocar, next);
            next.Index = _previous.Index + 1;
            _previous = next;
            AddTile(next, or);
            return next;
        }

        private void AddTile(Tile first, CardinalTileOrientation? ori)
        {
            _ccordarray[first.X, first.Y] = first;
            _ccords.Add(first);

            //TileCreationLogs.Insert(0, $"Tile created : x={first.X}, y={first.Y}");
        }

        private void CalculatePlacement(Tile next)
        {
            next.PlacementLeft = next.X * _areainfos.TileSize;
            next.PlacementTop = next.Y * _areainfos.TileSize;
        }

        private void ComputeAreaOccupation(OccupationArea ocar, Tile next)
        {
            if (ocar == null) return;

            var totalsurface = _areainfos.HorizontalTileOccupationThresold * _areainfos.VerticalTileOccupationThresold;
            var nextSurface = _areainfos.TileSize * _areainfos.TileSize;
            var occupBefore = ocar.Occupation;

            ocar.Occupation = ocar.Occupation + ((double)nextSurface * 100 / totalsurface);

            if (occupBefore < _areainfos.OccupationPercentageThresold && ocar.Occupation>= _areainfos.OccupationPercentageThresold)
            {
                TileCreationLogs.Insert(0, $"Occupation reached :{ocar.Name} | {ocar.Color} {ocar.Occupation} (Start={ocar.StartPoint})");

            }
        }

        private void CreateRandomizers()
        {
            _cardinalTileGen = new Random(DateTime.Now.Millisecond);
            _deadEndRestart = new Random(DateTime.Now.Second);
            _corridorBalance = new Random((int)(DateTime.Now.Minute / 0.8));
            _dispersionBalance = new Random((int)(DateTime.Now.Millisecond / 5.2));
        }
        private OccupationArea GetOccupationArea(Tile next)
        {
            return _areainfos.OccupationAreas.FirstOrDefault(o => IsTileInside(o, next));
        }

        private CardinalTileOrientation GetOrientation(CardinalTileOrientation? force)
        {
            if (force.HasValue)
                return force.Value;
            return (CardinalTileOrientation)_cardinalTileGen.Next(0, 4);
        }

        private bool IsCoordOccupiedOrOut(Tile next)
        {
            try
            {
                return

                    (next.X < 0 || next.X >= _areainfos.AreaWidth)
                    ||
                    (next.Y < 0 || next.Y >= _areainfos.AreaHeight)
                    ||
                    _ccordarray[next.X, next.Y] != null
                    ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool IsOccupationThresoldReached(Tile next, OccupationArea ocar, ref CardinalTileOrientation? orientationForce)
        {
            if (ocar != null)
            {

                if (ocar.Occupation >= _areainfos.OccupationPercentageThresold)
                {

                    orientationForce = GetOrientation(null);
                    TileCreationLogs.Insert(0, $"Force to {orientationForce}");

                    return true;
                }
                return false;
            }

            return false;
        }

        private bool IsTileInside(OccupationArea or, Tile next)
        {
            return ((next.PlacementLeft > or.StartPoint.X && next.PlacementLeft <= or.StartPoint.X + _areainfos.HorizontalTileOccupationThresold) && (next.PlacementTop > or.StartPoint.Y && next.PlacementTop <= or.StartPoint.Y + _areainfos.VerticalTileOccupationThresold));
        }

        private bool MustBeCorridor(Tile next)
        {
            //si balance vers corridors
            var freesides = ReturnFreeSides(next);
            var favorizeCorridors = _corridorBalance.NextDouble() < _areainfos.CorridorBalance;
            if (favorizeCorridors)
            {
                return !(freesides >= _areainfos.CorridorFreeTilesThresold);
            }

            return false;
        }

        private bool MustDisperse(Tile next)
        {
            var thresold = _areainfos.DispersionThresold;
            return false;

        }
        private int ReturnFreeSides(Tile tile)
        {
            //var right = (tile.X + 1 >= _areainfos.AreaWidth || _ccordarray[tile.X + 1, tile.Y] != null) ? 0 : 1;
            //var left = tile.X - 1 < 0 || _ccordarray[tile.X - 1, tile.Y] != null ? 0 : 1;
            //var down = tile.Y + 1 >= _areainfos.AreaHeight || _ccordarray[tile.X, tile.Y + 1] != null ? 0 : 1;
            //var up = tile.Y - 1 < 0 || _ccordarray[tile.X, tile.Y - 1] != null ? 0 : 1;



            int reult = 0;
            foreach (var func in GetFreetileCheck(FreeTileCheckMode.IncludeDiagonals))
            {
                reult += func(tile, 1);
            }
            return reult;
        }

        private int IsFreeFromUp(Tile tile, int radius)
        {
            return tile.Y - radius < 0 || _ccordarray[tile.X, tile.Y - radius] != null ? 0 : 1;
        }

        private int IsFreeFromDown(Tile tile, int radius)
        {
            return tile.Y + radius >= _areainfos.AreaHeight || _ccordarray[tile.X, tile.Y + radius] != null ? 0 : 1;
        }
        private int IsFreeFromLeft(Tile tile, int radius)
        {
            return tile.X - radius < 0 || _ccordarray[tile.X - radius, tile.Y] != null ? 0 : 1;
        }
        private int IsFreeFromRight(Tile tile, int radius)
        {
            return (tile.X + radius >= _areainfos.AreaWidth || _ccordarray[tile.X + radius, tile.Y] != null) ? 0 : 1;

        }

        private int IsFreeFromupRight(Tile tile, int radius)
        {
            return (tile.X + radius >= _areainfos.AreaWidth || tile.Y - radius < 0 || _ccordarray[tile.X + radius, tile.Y - radius] != null) ? 0 : 1;

        }

        private int IsFreeFromupLeft(Tile tile, int radius)
        {
            return (tile.X - radius < 0 || tile.Y - radius < 0 || _ccordarray[tile.X - radius, tile.Y - radius] != null) ? 0 : 1;

        }
        private int IsFreeFromDownLeft(Tile tile, int radius)
        {
            return (tile.X - radius < 0 || tile.Y + radius >= _areainfos.AreaHeight || _ccordarray[tile.X - radius, tile.Y + radius] != null) ? 0 : 1;

        }

        private int IsFreeFromDownRight(Tile tile, int radius)
        {
            return (tile.X + radius >= _areainfos.AreaWidth || tile.Y + radius >= _areainfos.AreaHeight || _ccordarray[tile.X + radius, tile.Y + radius] != null) ? 0 : 1;

        }

        List<Func<Tile, int, int>> GetFreetileCheck(FreeTileCheckMode mode)
        {
            var baselist = new List<Func<Tile, int, int>> { IsFreeFromLeft, IsFreeFromRight, IsFreeFromDown, IsFreeFromUp };
            switch (mode)
            {
                case FreeTileCheckMode.AdjacentsOnly:
                    return baselist;
                case FreeTileCheckMode.IncludeDiagonals:
                    baselist.Add(IsFreeFromDownLeft);
                    baselist.Add(IsFreeFromDownRight);
                    baselist.Add(IsFreeFromupLeft);
                    baselist.Add(IsFreeFromupRight);
                    return baselist;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

        enum FreeTileCheckMode
        {
            AdjacentsOnly,
            IncludeDiagonals
        }
    }
}
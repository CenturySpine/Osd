using O.S.D.Models;
using System.Collections.ObjectModel;
using O.S.D.Ui.Common;

namespace O.S.D.GameManagement
{
    public delegate void GenerationEnded(bool RealStop);

    internal class GameAreaManager :NotifierBase, IGameAreaManager
    {
        public IAreaInfos Areainfos { get; }

        public event GenerationEnded GenerationEnd;

        private readonly ITileDiscoveringService _tileDisc;
        private int _tileCount;

        public GameAreaManager(ITileDiscoveringService tileDisc, IAreaInfos areainfos)
        {
            Areainfos = areainfos;
            _tileDisc = tileDisc;

            Tiles = new ObservableCollection<Tile> { };
            Discover(false);
            Discover(false);
        }

        public ObservableCollection<Tile> Tiles { get; set; }

        public void Discover(bool mustRestart)
        {
            var tile = _tileDisc.New(mustRestart);
            if (tile == null)
            {
                OnGenerationEnd(false);
                return;
            }
            Tiles.Add(tile);
            TileCount = Tiles.Count;
            if (TileCount == Areainfos.MaxtileNumber)
            {
                OnGenerationEnd(true);
                return;
            }
        }

        public int TileCount
        {
            get { return _tileCount; }
            set { _tileCount = value;Notify(); }
        }

     

        public void Clear()
        {
            Tiles.Clear();
            _tileDisc.Clear();
            TileCount = 0;
            Discover(false);
            Discover(false);

        }

        protected virtual void OnGenerationEnd(bool b)
        {
            GenerationEnd?.Invoke(b);
        }
    }
}
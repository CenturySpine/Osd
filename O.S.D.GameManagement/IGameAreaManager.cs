using O.S.D.Models;
using System.Collections.ObjectModel;

namespace O.S.D.GameManagement
{
    public interface IGameAreaManager
    {
        ObservableCollection<Tile> Tiles { get; set; }
        IAreaInfos Areainfos { get; }
        int TileCount { get; set; }

        event GenerationEnded GenerationEnd;

        void Discover(bool mustRestart);

        void Clear();
    }
}
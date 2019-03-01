using O.S.D.Models;
using System.Collections.ObjectModel;

namespace O.S.D.GameManagement
{
    public interface ITileDiscoveringService
    {
        Tile New(bool mustRestart, CardinalTileOrientation? force = null);

        ObservableCollection<string> TileCreationLogs { get; }

        void Clear();
    }
}
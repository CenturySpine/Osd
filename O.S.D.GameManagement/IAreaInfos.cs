using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace O.S.D.GameManagement
{
    public interface IAreaInfos
    {
        int AreaHeight { get; }
        int AreaWidth { get; }
        int TileSize { get; }
        ObservableCollection<OccupationArea> OccupationAreas { get; set; }
        double OccupationPercentageThresold { get; set; }
        int HorizontalTileOccupationThresold { get; set; }
        int VerticalTileOccupationThresold { get; set; }
        int MaxtriesToReachNewOccupationArea { get; set; }
        double CorridorBalance { get; set; }
        double DispersionPropension { get; set; }
        int DispersionThresold { get; set; }
        int CorridorFreeTilesThresold { get; set; }
        int MaxtileNumber { get; set; }

        int StartY { get; set; }
        int StartX { get; set; }
        void ClearOccupations();
    }
}
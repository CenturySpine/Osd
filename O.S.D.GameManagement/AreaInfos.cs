using System.Collections.ObjectModel;
using System.Windows;
using O.S.D.Ui.Common;

namespace O.S.D.GameManagement
{
    internal class AreaInfos : NotifierBase, IAreaInfos
    {
        private double _corridorBalance;
        private int _corridorFreeTilesThresold;
        private int _startY;
        private int _startX;
        private int _maxtileNumber;

        public AreaInfos()
        {
            AreaHeight = 1000;
            AreaWidth = 1000;
            TileSize = 10;
            VerticalTileOccupationThresold = AreaHeight / 5;
            HorizontalTileOccupationThresold = AreaWidth / 5;
            OccupationPercentageThresold = 20.0;
            OccupationAreas = new ObservableCollection<OccupationArea>();
            MaxtriesToReachNewOccupationArea = 200;

            CorridorBalance = 0.95;
            CorridorFreeTilesThresold = 7;
            DispersionPropension = 0.7;
            DispersionThresold = 2;

            MaxtileNumber = 500;

            StartX = 5;
            StartY = 5;

            GenerateOccupationAreas();
        }

        public int MaxtileNumber
        {
            get { return _maxtileNumber; }
            set { _maxtileNumber = value; Notify(); }
        }

        public int StartY
        {
            get { return _startY; }
            set { _startY = value; Notify(); }
        }

        public int StartX
        {
            get { return _startX; }
            set { _startX = value; Notify(); }
        }

        public void ClearOccupations()
        {
            foreach (var oa in OccupationAreas)
            {
                oa.Occupation = 0.0;
            }
        }

        public int CorridorFreeTilesThresold
        {
            get { return _corridorFreeTilesThresold; }
            set { _corridorFreeTilesThresold = value; Notify(); }
        }

        public int DispersionThresold { get; set; }

        public double DispersionPropension { get; set; }

        public double CorridorBalance
        {
            get { return _corridorBalance; }
            set { _corridorBalance = value; Notify(); }
        }

        public int MaxtriesToReachNewOccupationArea { get; set; }

        private void GenerateOccupationAreas()
        {
            //OccupationArea prev = null;
            for (int i = 0; i < AreaWidth; i = i + HorizontalTileOccupationThresold)
            {
                for (int j = 0; j < AreaHeight; j = j + VerticalTileOccupationThresold)
                {
                    var occAr = new OccupationArea
                    {
                        StartPoint = new Point(i, j),
                        Name = $"{i},{j}",
                        Height = VerticalTileOccupationThresold,
                        Width = HorizontalTileOccupationThresold,
                        Color = ColorHelper.get()
                    };
                    OccupationAreas.Add(occAr);
                    //prev = occAr;
                }
            }
        }

        public double OccupationPercentageThresold { get; set; }

        public int HorizontalTileOccupationThresold { get; set; }

        public int VerticalTileOccupationThresold { get; set; }

        public int AreaHeight { get; set; }
        public int AreaWidth { get; set; }
        public int TileSize { get; set; }

        public ObservableCollection<OccupationArea> OccupationAreas { get; set; }
    }
}
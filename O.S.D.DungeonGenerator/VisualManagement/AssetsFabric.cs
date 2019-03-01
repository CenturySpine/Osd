using O.S.D.DungeonGenerator.EnvironementManagement;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using O.S.D.DungeonGenerator.DungeonGeneration;

namespace O.S.D.DungeonGenerator.VisualManagement
{
    internal class AssetsFabric
    {
        private readonly DungeonParameters _dp;

        public AssetsFabric(DungeonParameters dp)
        {
            _dp = dp;
        }
        internal FrameworkElement GetTile(TileType type, int xpos, int ypos)
        {
            return new TileAsset(type, xpos, ypos);
        }

        public Border GetPlayer()
        {
            Border grd = new Border() { Width = _dp.TileSize, Height = _dp.TileSize };
            Ellipse el = new Ellipse() { Fill = new SolidColorBrush(Colors.White) };
            //el.VerticalAlignment = VerticalAlignment.Center;
            //el.HorizontalAlignment = HorizontalAlignment.Center;
            grd.Child = el;
            return grd;
        }
    }
}
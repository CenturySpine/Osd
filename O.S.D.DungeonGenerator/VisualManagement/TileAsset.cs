using O.S.D.DungeonGenerator.EnvironementManagement;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace O.S.D.DungeonGenerator.VisualManagement
{
    internal class TileAsset : Grid
    {
        public int Xpos { get; }
        public int Ypos { get; }

        public TileAsset(TileType type, int xpos, int ypos)
        {
            Xpos = xpos;
            Ypos = ypos;
            switch (type)
            {
                case TileType.None:
                    this.Children.Add(MakeTile(Colors.DeepSkyBlue, Colors.White, new Thickness(0)));
                    break;

                case TileType.Room:
                    this.Children.Add(MakeTile(Colors.Black, Colors.Red, new Thickness(0)));
                    break;

                case TileType.Corridor:
                    this.Children.Add(MakeTile(Colors.Maroon, Colors.DarkOrchid, new Thickness(0)));
                    break;

                case TileType.Pilar:
                    this.Children.Add(MakeTile(Colors.Yellow, Colors.Black, new Thickness(0)));
                    break;

                case TileType.ExitEntrance:
                    this.Children.Add(MakeTile(Colors.Chartreuse, Colors.Orange, new Thickness(0)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private FrameworkElement MakeTile(Color backcolor, Color bordercolor, Thickness borderEdgesThickness)
        {
            //Grid g = new Grid();
            System.Windows.Controls.Border b = new Border
            {
                Background = new SolidColorBrush(backcolor),
                BorderBrush = new SolidColorBrush(bordercolor),
                BorderThickness = borderEdgesThickness
            };
            //g.Children.Add(b);
            return b;
        }
    }
}
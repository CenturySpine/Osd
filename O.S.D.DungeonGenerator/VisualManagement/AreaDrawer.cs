using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace O.S.D.DungeonGenerator.VisualManagement
{
    internal class AreaDrawer
    {
        private readonly DungeonParameters _dp;
        private readonly Generator _gen;
        private readonly Canvas _mainArea;
        private readonly AssetsFabric _assests;


        private Border _playerSprite;
        private List<TileAsset> _tilesVisibilities;

        public AreaDrawer(DungeonParameters dp, Generator gen, Canvas mainArea, AssetsFabric assests)
        {
            _dp = dp;
            _gen = gen;
            _mainArea = mainArea;
            _assests = assests;
            _mainArea.Focusable = true;

        }

        public void ChangePlayerStance(Player player)
        {
            _playerSprite.Padding = player.Stance == PlayerStance.Up ? new Thickness(0) : new Thickness(Math.Round((double)_dp.TileSize /4));
        }

        public void Clear()
        {
            _mainArea.Children.Clear();
        }

        public void DrawDungeon()
        {
            _mainArea.Width = _dp.PixelWidth;
            _mainArea.Height = _dp.PixelHeight;

            for (int i = 0; i < _dp.AreaWidth; i++)
            {
                for (int j = 0; j < _dp.AreaHeight; j++)
                {
                    FrameworkElement fe;
                    var tile = _gen.TilesMatrix[i, j];

                    fe = _assests.GetTile(tile.TileType, i, j);
                    if (fe == null)
                    {
                        throw new InvalidOperationException("Cant find tile asset");
                    }
                    fe.Width = _dp.TileSize;
                    fe.Height = _dp.TileSize;
                    _mainArea.Children.Add(fe);
                    Canvas.SetLeft(fe, i * _dp.TileSize);
                    Canvas.SetTop(fe, j * _dp.TileSize);
                    fe.Visibility = Visibility.Hidden;
                }
            }
        }

        public void Focus()
        {
            _mainArea.Focus();
            Keyboard.Focus(_mainArea);
        }

        public void HideNotDiscoveredTiles(Player player)
        {
            _tilesVisibilities.ForEach(t => t.Visibility = Visibility.Hidden);
            ChangeTilesVis(player);
        }

        public void MovePlayer(Player player)
        {
            //Thread.Sleep(player.GetModifiedVelocity());
            InternalMovePlayer(player);
            ChangeTilesVis(player);
        }

        public void PlacePlayer(Player player)
        {
            _playerSprite = _assests.GetPlayer();

            _mainArea.Children.Add(_playerSprite);
            InternalMovePlayer(player);
            _tilesVisibilities = _mainArea.Children.OfType<TileAsset>().ToList();

            ChangeTilesVis(player);

            var centeroffsetX = player.XPos - _mainArea.Width / 2 + (_mainArea.Width - player.XPos);
            var centeroffsetY = player.YPos - _mainArea.Height / 2 + (_mainArea.Height - player.YPos);
            _mainArea.LayoutTransform = new TranslateTransform(centeroffsetX, centeroffsetY);
        }

        public void ShowAlltiles()
        {
            _tilesVisibilities.ForEach(t => t.Visibility = Visibility.Visible);
        }

        public void Zoom(double eNewValue, Player player)
        {
            _mainArea.RenderTransformOrigin = new Point(player.XPos, player.YPos);
            _mainArea.LayoutTransform = new ScaleTransform() { ScaleX = eNewValue, ScaleY = eNewValue };
        }

        private void ChangeTilesVis(Player player)
        {
            int width; int height;

            width = height = player.VisionRadius * 2 + 1;

            var xstart = player.XPos - player.VisionRadius;
            var ystaert = player.YPos - player.VisionRadius;

            foreach (var grid in _tilesVisibilities.Where(t => t.Visibility == Visibility.Hidden))
            {
                for (int i = xstart; i < xstart + width; i++)
                {
                    for (int j = ystaert; j < ystaert + height; j++)
                    {
                        if (grid.Xpos == i && grid.Ypos == j)
                        {
                            grid.Visibility = Visibility.Visible;
                        }
                    }
                }
            }
        }

        private void InternalMovePlayer(Player player)
        {
            Canvas.SetLeft(_playerSprite, player.XPos * _dp.TileSize);
            Canvas.SetTop(_playerSprite, player.YPos * _dp.TileSize);
        }
    }
}
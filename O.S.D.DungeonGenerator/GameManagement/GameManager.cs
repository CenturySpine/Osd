using O.S.D.DungeonGenerator.CharacterManagement;
using O.S.D.DungeonGenerator.DungeonGeneration;
using O.S.D.DungeonGenerator.KeysInputManagement;
using O.S.D.DungeonGenerator.VisualManagement;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace O.S.D.DungeonGenerator.GameManagement
{
    internal class GameManager
    {
        private AssetsFabric _assests;
        private DungeonParameters _dp;
        private Player _player;
        private int maxtries = 20;
        private int _tries = 0;



        internal void Initialize(Canvas mainArea)
        {
            _dp = new DungeonParameters();
            _assests = new AssetsFabric(_dp);
            Gen = new Generator(_dp);
            Drawer = new AreaDrawer(_dp, Gen, mainArea, _assests);
            _player = new Player();
            InputManager = new KeysInputBehaviorFactory(Gen, Drawer);
        }

        public AreaDrawer Drawer { get; set; }
        public Generator Gen { get; set; }
        public KeysInputBehaviorFactory InputManager { get; set; }
        public double ZoomValue { get; set; }

        public void HideNotDiscoveredTiles()
        {
            Drawer.HideNotDiscoveredTiles(_player);
        }

        public bool ManageInput(Key eKey)
        {
            return InputManager.IsManaged(eKey, _player);
        }

        public void ShowAlltiles()
        {
            Drawer.ShowAlltiles();
        }

        public void Zoom(double eNewValue)
        {
            Drawer.Zoom(eNewValue, _player);
        }

        internal async Task ClearAndGen()
        {
            _tries = 0;
            await DoClearAndGen();
        }

        private async Task DoClearAndGen()
        {
            Gen.Clear();
            Drawer.Clear();
            try
            {
                await Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal async Task Start()
        {
            try
            {
                await Gen.Genrate();

                Drawer.DrawDungeon();

                Gen.PlacePlayerStartPosition(_player);
                Drawer.PlacePlayer(_player);
                ZoomValue = 1.0;
            }
            catch (Generator.GenerationabortedException)
            {
                _tries++;
                if (_tries >= maxtries)
                {
                    return;
                }
                DoClearAndGen();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
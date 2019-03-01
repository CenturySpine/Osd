using System;
using System.Windows;
using O.S.D.DungeonGenerator.GameManagement;

namespace O.S.D.DungeonGenerator
{
    public partial class MainWindow : Window
    {
        private GameManager _gman;


        internal MainWindow(GameManager gman)
        {
            _gman = gman;
            InitializeComponent();


            _gman.Initialize(main_area);
            logslist.ItemsSource = _gman.Gen.Logs;

            PreviewKeyDown += MainWindow_KeyDown;
        }

        private async void Btn_gen_OnClick(object sender, RoutedEventArgs e)
        {
            
            await _gman.ClearAndGen();
        }

        private void Btn_hidenosee_OnClick(object sender, RoutedEventArgs e)
        {
            _gman.HideNotDiscoveredTiles();
        }

        private void BtnShowall_OnClick(object sender, RoutedEventArgs e)
        {
            _gman.ShowAlltiles();
        }



        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (_gman.ManageInput(e.Key))
            {
                e.Handled = true;
            }
        }

        

        private void Zoom_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _gman.Zoom(e.NewValue);
        }
    }
}
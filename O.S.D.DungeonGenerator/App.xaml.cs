using System;
using System.Windows;
using O.S.D.DungeonGenerator.GameManagement;

namespace O.S.D.DungeonGenerator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameManager _gman;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _gman = new GameManager();

            MainWindow mw = new MainWindow(_gman);
            Application.Current.MainWindow = mw;
            mw.Show();

            try
            {
               await _gman.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
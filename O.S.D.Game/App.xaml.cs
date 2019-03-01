using System.Windows;
using SimpleInjector;

namespace O.S.D.Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Container _container;
        private GameBoostrapper _boot;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _container = new Container();
            _boot = new GameBoostrapper();
            _boot.Register(_container);
            _boot.Initialize(_container);
        }
    }
}

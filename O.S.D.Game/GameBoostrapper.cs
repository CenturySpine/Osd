using System.Windows;
using O.S.D.Common;
using O.S.D.GameManagement;
using SimpleInjector;

namespace O.S.D.Game
{
    class GameBoostrapper : OsdBootstrapper
    {




        public override void Register(Container maincontainer)
        {
             new GameManagementBoostrapper().Register(maincontainer);
            maincontainer.Register<IGameHub, GameHub>();
            maincontainer.Register<MainWindow>();

        }

        public override void Initialize(Container maincontainer)
        {
            maincontainer.GetInstance<GameManagementBoostrapper>().Initialize(maincontainer);

            StartApplication(maincontainer);
        }

        private void StartApplication(Container maincontainer)
        {
            Application.Current.MainWindow = maincontainer.GetInstance<MainWindow>();
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            if (Application.Current.MainWindow != null) Application.Current.MainWindow.Show();
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
            e.Handled = true;
        }
    }
}

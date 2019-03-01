using System;
using System.Threading;
using System.Windows;
using O.S.D.GameManagement;
using O.S.D.Ui.Common;

namespace O.S.D.Game
{
    class GameHub: NotifierBase, IGameHub
    {
        private Timer _discoverTimer;
        private bool _restartFromExisting = false;
        public IGameAreaManager AreaManager { get; }
        public ITileDiscoveringService TileSrv { get; }
        public IAreaInfos Areainfos { get; }
        public DelegateCommand CmdStopManualDiscover { get; }

        public bool DiscoverManual
        {
            get { return _discoverManual; }
            set { _discoverManual = value;Notify(); }
        }

        public DelegateCommand CmpdClearAllData { get; set; }

        public DelegateCommand CmdManualDiscover { get; }

        public GameHub(IGameAreaManager areaManager, ITileDiscoveringService tileSrv, IAreaInfos areainfos)
        {
            AreaManager = areaManager;
            AreaManager.GenerationEnd += AreaManager_GenerationEnd;
            TileSrv = tileSrv;
            Areainfos = areainfos;
            CmdManualDiscover = new DelegateCommand(ExecuteManualDiscover);
            CmdStopManualDiscover = new DelegateCommand(StopDiscover);
            CmpdClearAllData = new DelegateCommand(ExecuteClearAllData);
        }

        private void ExecuteClearAllData()
        {
            currentGentTry = 0;
            TileSrv.Clear();
            AreaManager.Clear();
        }

        private void StopDiscover()
        {
            if (_discoverTimer != null)
            {
                _discoverTimer.Dispose();
                _discoverTimer = null;
            }
        }

        private int maxGenTries = 200;
        private int currentGentTry = 0;
        private bool _discoverManual;

        private void AreaManager_GenerationEnd(bool realStop)
        {
            if (_discoverTimer != null)
            {
                _discoverTimer.Dispose();
                _discoverTimer = null;
                //MessageBox.Show("Generation Ended");
                if(realStop)return;
                _restartFromExisting = true;
                currentGentTry++;
                if (currentGentTry <= maxGenTries)
                {
                    ExecuteManualDiscover();
                }
            }
        }

        private void ExecuteManualDiscover()
        {
            if (DiscoverManual)
            {
                AreaManager.Discover(_restartFromExisting);
            }
            else
            {
                _discoverTimer = new Timer(Ondiscover, null, 500, 50);
            }
            
        }

        private void Ondiscover(object state)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                AreaManager.Discover(_restartFromExisting);

            }));
        }
    }
}
using O.S.D.GameManagement;

namespace O.S.D.Game
{
    public interface IGameHub
    {
        IGameAreaManager AreaManager { get; }
        DelegateCommand CmdManualDiscover { get; }
        ITileDiscoveringService TileSrv { get; }
        DelegateCommand CmdStopManualDiscover { get; }
        bool DiscoverManual { get; set; }
        DelegateCommand CmpdClearAllData { get; set; }
        IAreaInfos Areainfos { get; }
    }
}
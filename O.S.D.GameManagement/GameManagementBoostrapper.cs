using O.S.D.Common;
using SimpleInjector;

namespace O.S.D.GameManagement
{
    public class GameManagementBoostrapper : OsdBootstrapper
    {
        public override void Register(Container maincontainer)
        {
            maincontainer.RegisterSingleton(this);
            maincontainer.Register<IGameAreaManager, GameAreaManager>(Lifestyle.Singleton);
            maincontainer.Register<ITileDiscoveringService, TileDiscoveringService>(Lifestyle.Singleton);
            maincontainer.Register<IAreaInfos, AreaInfos>(Lifestyle.Singleton);
        }

        public override void Initialize(Container maincontainer)
        {
        }
    }
}
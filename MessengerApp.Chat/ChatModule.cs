using Prism.Modularity;
using Prism.Regions;

namespace MessengerApp.Chat
{
    public class ChatModule : IModule
    {
        IRegionManager _regionManager;

        public ChatModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(Views.Chat));
        }
    }
}

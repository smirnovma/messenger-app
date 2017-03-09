using MessengerApp.Interfaces.Security;
using MessengerApp.Security.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace MessengerApp.Security
{
    public class SecurityModule : IModule
    {
        IRegionManager _regionManager;
        private readonly IUnityContainer _container;

        public SecurityModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<ILoginView, LoginWindow>();
            _container.RegisterType<RegistrationWindow>();
        }
    }
}

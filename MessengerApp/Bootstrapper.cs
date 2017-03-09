using Prism.Unity;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using MessengerApp.Views;
using MessengerApp.Chat;
using MessengerApp.Security;
using MessengerApp.Interfaces.Security;
using MessengerApp.Security.Views;
using System;
using System.Diagnostics;

namespace MessengerApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
            Application.Current.Exit += delegate { Process.GetCurrentProcess().Kill(); };
            Window window = new Window
            {
                Title = "Enter login and password",
                Content = Container.Resolve<LoginWindow>(),
                Height = 120,
                Width = 320,
                ResizeMode = ResizeMode.NoResize
            };
            //window.Closed += delegate { Environment.Exit(0); };
            //window.WindowStyle = WindowStyle.None;
            window.ShowDialog();
        }

        protected override void ConfigureModuleCatalog()
        {
            ModuleCatalog catalog = (ModuleCatalog)ModuleCatalog;
            catalog.AddModule(typeof(ChatModule));
            catalog.AddModule(typeof(SecurityModule));
        }
    }
}

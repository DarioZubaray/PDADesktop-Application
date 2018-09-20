using PDADesktop.Classes;
using PDADesktop.Classes.Devices;
using PDADesktop.ViewModel;
using StructureMap;
using System;
using System.Configuration;

namespace PDADesktop.Classes
{
    class MyContainerInitializer : Registry
    {
        public MyContainerInitializer()
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
            });

            ForConcreteType<MainWindowViewModel>().Configure.Singleton();

            ForConcreteType<BuscarLotesViewModel>().Configure.Singleton();
            ForConcreteType<CentroActividadesViewModel>().Configure.Singleton();
            ForConcreteType<LoginViewModel>().Configure.Singleton();
            ForConcreteType<VerAjustesViewModel>().Configure.Singleton();

            string deviceHandler = ConfigurationManager.AppSettings.Get("DEVICE_HANDLER");
            if (deviceHandler.Equals(MyAppProperties.DESKTOP_ADAPTER, StringComparison.InvariantCultureIgnoreCase))
            {
                this.For<IDeviceHandler>().Use<DesktopAdapter>().Named("desktopAdapter");
            }
            else
            {
                this.For<IDeviceHandler>().Use<MotoAdapter>().Named("motoAdapter");
            }
        }
    }
}

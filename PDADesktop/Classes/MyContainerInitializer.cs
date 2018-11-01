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

            ForConcreteType<LoginViewModel>().Configure.Singleton();

            ForConcreteType<CentroActividadesViewModel>().Configure.Singleton();

            ForConcreteType<VerAjustesRealizadosViewModel>().Configure.Singleton();
            ForConcreteType<VerAjustesModificarViewModel>().Configure.Singleton();
            ForConcreteType<VerAjustesInformadosViewModel>().Configure.Singleton();

            ForConcreteType<VerDetallesRecepcionViewModel>().Configure.Singleton();
            ForConcreteType<ImprimirRecepcionViewModel>().Configure.Singleton();

            string deviceHandler = ConfigurationManager.AppSettings.Get(Constants.DEVICE_HANDLER);
            if (deviceHandler.Equals(Constants.DESKTOP_ADAPTER, StringComparison.InvariantCultureIgnoreCase))
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

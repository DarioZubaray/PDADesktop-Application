using PDADesktop.ViewModel;
using StructureMap;

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

        }
    }
}

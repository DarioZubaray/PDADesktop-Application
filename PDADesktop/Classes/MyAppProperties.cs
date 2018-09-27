using PDADesktop.Model;
using PDADesktop.View;

namespace PDADesktop.Classes
{
    class MyAppProperties
    {
        public static string idSucursal { get; set; }
        public static string idLoteActual { get; set; }
        public static MainWindow window { get; set; }

        public static SincronizacionDtoDataGrid SelectedSynchronization { get; set; }
    }
}

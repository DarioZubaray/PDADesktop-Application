using PDADesktop.Model;
using PDADesktop.Model.Dto;
using PDADesktop.View;
using System.Collections.Generic;

namespace PDADesktop.Classes
{
    class MyAppProperties
    {
        public static string idSucursal { get; set; }
        public static string idLoteActual { get; set; }
        public static MainWindow window { get; set; }

        public static SincronizacionDtoDataGrid SelectedSynchronization { get; set; }
        public static List<Accion> accionesDisponibles { get; set; }
        public static List<Actividad> actividadesDisponibles { get; set; }

        public static bool isSynchronizationComplete { get; set; }
    }
}

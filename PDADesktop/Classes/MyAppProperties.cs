using PDADesktop.Model;
using PDADesktop.Model.Dto;
using PDADesktop.View;
using System.Collections.Generic;

namespace PDADesktop.Classes
{
    class MyAppProperties
    {
        public static string storeId { get; set; }
        public static string currentBatchId { get; set; }
        public static MainWindow window { get; set; }

        public static SincronizacionDtoDataGrid SelectedSync { get; set; }
        public static List<Accion> actionsEnables { get; set; }
        public static List<Actividad> activitiesEnables { get; set; }

        public static bool isSynchronizationComplete { get; set; }

        public static bool isSeeAdjustmentsWindowClosed { get; set; }
    }
}

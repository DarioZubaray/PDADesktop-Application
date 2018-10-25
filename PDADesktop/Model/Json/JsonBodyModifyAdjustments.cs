using System.Collections.ObjectModel;

namespace PDADesktop.Model.Json
{
    public struct JsonBodyModifyAdjustments
    {
        public long idSincronizacion { get; set; }
        public ObservableCollection<Ajustes> modificarAjustes { get; set; }
    }
}

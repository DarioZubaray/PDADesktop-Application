using System;

namespace PDADesktop.Model
{
    public class Recepcion
    {
        internal long idRecepcion { get; set; }
        internal Lote lote { get; set; }
        internal DateTime fechaRecepcion { get; set; }
        internal long numeroRemito { get; set; }
        internal DateTime FechaRemito { get; set; }
        internal string letra { get; set; }
        internal long sucursalRemito { get; set; }
        internal long centroEmisor { get; set; }
        internal long numeroPedido { get; set; }
        internal long numeroProveedor { get; set; }
        internal string descripcionProveedor { get; set; }
        internal long numeroRecepcion { get; set; }
        internal EstadoRecepcion estado { get; set; }
    }
}

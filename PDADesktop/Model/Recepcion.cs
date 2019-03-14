using System;

namespace PDADesktop.Model
{
    public class Recepcion
    {
        public long idRecepcion { get; set; }
        public Lote lote { get; set; }
        public string fechaRecep
        {
            get
            {
                return fechaRecepcion.ToString();
            }
            set
            {
                fechaRecepcion = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public DateTime fechaRecepcion { get; set; }
        public long numeroRemito { get; set; }
        public string remitoCompleto { get; set; }
        public string fechaRem
        {
            get
            {
                return FechaRemito.ToString();
            }
            set
            {
                FechaRemito = DateTime.ParseExact(value, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
        }
        public DateTime FechaRemito { get; set; }
        public string letra { get; set; }
        public long sucursalRemito { get; set; }
        public long centroEmisor { get; set; }
        public long numeroPedido { get; set; }
        public long numeroProveedor { get; set; }
        public string descripcionProveedor { get; set; }
        public long numeroRecepcion { get; set; }
        public EstadoRecepcion estado { get; set; }
    }
}

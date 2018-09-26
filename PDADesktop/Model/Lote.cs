using System;

namespace PDADesktop.Model
{
    public class Lote
    {
        public Int64 idLote { get; set; }
        public DateTime fecha { get; set; }
        public Int64 sucursal {get;set;}

        public Lote() { }
        public Lote(int idLote, DateTime fecha, int sucursal)
        {
            this.idLote = idLote;
            this.fecha = fecha;
            this.sucursal = sucursal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Model
{
    class Lote
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

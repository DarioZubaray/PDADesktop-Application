using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegacion.Model
{
    class Sincronizacion
    {
        public Int64 idSincronizacion { get; set; }
        public Lote lote { get; set; }
        public Actividad actividad { get; set; }
        public EstadoGenesix egx { get; set; }
        public EstadoPDA epda { get; set; }
        public EstadoGeneral egral { get; set; }
    }
}

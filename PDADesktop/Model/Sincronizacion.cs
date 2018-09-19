using System;

namespace PDADesktop.Model
{
    class Sincronizacion
    {
        public Int64 idSincronizacion { get; set; }
        public Lote lote { get; set; }
        public Actividad actividad { get; set; }
        public EstadoGenesix egx { get; set; }
        public EstadoPDA epda { get; set; }
        public EstadoGeneral egral { get; set; }

        public override string ToString() {
            return "Sincronizacion[" + idSincronizacion 
                + "]";
        }
    }
}

using System;

namespace PDADesktop.Model
{
    class Actividad
    {
        public Int64 idActividad { get; set; }
        public String descripcion { get; set; }
        public Accion accion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navegacion.Model
{
    class Actividad
    {
        public Int64 idActividad { get; set; }
        public String descripcion { get; set; }
        public Accion accion { get; set; }
    }
}

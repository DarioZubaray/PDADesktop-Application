using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Model
{
    class SincronizacionPOCO
    {
        public string lote { get; set; }
        public int idAccion { get; set; }
        public string accion { get; set; }
        public string fecha { get; set; }
        public int idActividad { get; set; }
        public string actividad { get; set; }
        public int idEstadoGenesix { get; set; }
        public string genesix { get; set; }
        public int idEstadoPda { get; set; }
        public string pda { get; set; }
        public int idEstadoGeneral { get; set; }
        public string estado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Model
{
    class Ajustes
    {
        public long ean { get; set; }
        public string fechaAjuste { get; set; }
        public string motivo { get; set; }
        public string perfilGenesix { get; set; }
        public long cantidad { get; set; }
        public string claveAjuste { get; set; }

        public Ajustes(long ean, string fechaAjuste, string motivo, long cantidad)
        {
            this.ean = ean;
            this.fechaAjuste = fechaAjuste;
            this.motivo = motivo;
            this.cantidad = cantidad;
        }
    }
}

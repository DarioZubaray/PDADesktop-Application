using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Model
{
    public class ArticuloRecepcion
    {
        public long EAN { get; set; }
        public double unidadesRecibidas { get; set; }
        public Recepcion recepcion { get; set; }
    }
}

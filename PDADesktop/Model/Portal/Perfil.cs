using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Model.Portal
{
    public class Perfil
    {
        public long activo { get; set; }
        public long codigoPerfil { get; set; }
        public string descripcion { get; set; }
        public ICollection<Object> paginasSet { get; set; }
    }
}

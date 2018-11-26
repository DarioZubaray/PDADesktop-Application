using System;
using System.Collections.Generic;

namespace PDADesktop.Model.Portal
{
    public class User
    {
        public bool IDM { get; set; }
        public List<Object> access { get; set; }
        public string apellido { get; set; }
        public string cuilCuit { get; set; }
        public string direccion { get; set; }
        string dni { get; set; }
        string empresa { get; set; }
        public string fechaEgreso { get; set; }
        public string fechaIngreso { get; set; }
        public long id { get; set; }
        public string legajo { get; set; }
        public List<long> listaNumeroPerfil { get; set; }
        public string mail { get; set; }
        public long multipunto { get; set; }
        public string nombre { get; set; }
        public string password { get; set; }
        public Object perfilConsolidado { get; set; }
        public ICollection<Object> perfilesSet { get; set; }
        public List<Object> profiles { get; set; }
        public string puesto { get; set; }
        public ICollection<Object> puntosSecundarios { get; set; }
        public string sector { get; set; }
        public long sucursal { get; set; }
        public string superiorDN { get; set; }
        public string telefono { get; set; }
        public bool temporalPassword { get; set; }
        public string tipoEmpleado { get; set; }
        public string unidadNegocio { get; set; }
        public string userIDM { get; set; }
        public string userName { get; set; }
        public string usuarioRed { get; set; }
    }
}

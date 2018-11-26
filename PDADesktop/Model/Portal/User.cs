using System;
using System.Collections.Generic;

namespace PDADesktop.Model.Portal
{
    public class User
    {
        bool IDM { get; set; }
        List<Object> access { get; set; }
        string apellido { get; set; }
        string cuilCuit { get; set; }
        string direccion { get; set; }
        string dni { get; set; }
        string empresa { get; set; }
        DateTime fechaEgreso { get; set; }
        DateTime fechaIngreso { get; set; }
        long id { get; set; }
        string legajo { get; set; }
        List<long> listaNumeroPerfil { get; set; }
        string mail { get; set; }
        long multipunto { get; set; }
        string nombre { get; set; }
        string password { get; set; }
        Object perfilConsolidado { get; set; }
        ICollection<Object> perfilesSet { get; set; }
        List<Object> profiles { get; set; }
        string puesto { get; set; }
        ICollection<Object> puntosSecundarios { get; set; }
        string sector { get; set; }
        long sucursal { get; set; }
        string superiorDN { get; set; }
        string telefono { get; set; }
        bool temporalPassword { get; set; }
        string tipoEmpleado { get; set; }
        string unidadNegocio { get; set; }
        string userIDM { get; set; }
        string userName { get; set; }
        string usuarioRed { get; set; }
    }
}

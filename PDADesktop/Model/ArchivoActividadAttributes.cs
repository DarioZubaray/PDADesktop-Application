using System;

namespace PDADesktop.Model
{
    public class ArchivoActividadAttributes : Attribute
    {
        public string idActividad { get; set; }
        public string nombreArchivo { get; set; }
        public string descripcion { get; set; }
        public bool maestro { get; set; }

        public ArchivoActividadAttributes(string idActividad, string nombreArchivo, string descripcion, bool maestro)
        {
            this.idActividad = idActividad;
            this.nombreArchivo = nombreArchivo;
            this.descripcion = descripcion;
            this.maestro = maestro;
        }
    }
}

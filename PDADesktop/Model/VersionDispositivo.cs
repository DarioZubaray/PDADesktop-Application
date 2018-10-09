using System;
using System.Collections.Generic;

namespace PDADesktop.Model
{
    public class VersionDispositivo
    {
        public bool habilitada { get; set; }
        public int idDispositivo { get; set; }
        public string idVersionDispositivo { get; set; }
        public string version { get; set; }
        public List<VersionArchivo> versiones { get; set; }
    }
}

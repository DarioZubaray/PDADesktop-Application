using System;
using System.Collections.Generic;

namespace PDADesktop.Model
{
    public class VersionDispositivo
    {
        public long id { get; set; }
        public Dispositivo dispositivo { get; set; }
        public string version { get; set; }
        public Boolean habilitada { get; set; }

        public ISet<VersionArchivo> versionesArchivos { get; set; }
    }
}

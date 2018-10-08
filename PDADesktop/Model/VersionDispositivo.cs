using System;
using System.Collections.Generic;

namespace PDADesktop.Model
{
    public class VersionDispositivo
    {
        public long idVersion { get; set; }
        public string dispositivo { get; set; }
        public string version { get; set; }
        public bool habilitada { get; set; }

        public List<VersionArchivo> versionesArchivos { get; set; }
    }
}

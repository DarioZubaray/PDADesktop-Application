using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Utils
{
    class MaestrosUtils
    {
        public static string GetMasterFileName(int idActividad)
        {
            string masterFile = null;
            switch (idActividad)
            {
                case 101:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_CONTROL_PRECIOS_CON_UBICACIONES");
                    break;
                case 102:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_CONTROL_PRECIOS_SIN_UBICACIONES");
                    break;
                case 103:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_AJUSTES");
                    break;
                case 104:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_RECEPCIONES");
                    break;
                case 105:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_IMPRESION_ETIQUETAS");
                    break;
                case 201:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_ARTICULOS");
                    break;
                case 202:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_UBICACIONES");
                    break;
                case 203:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_UBICACION_ARTICULOS");
                    break;
                case 204:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_PEDIDOS");
                    break;
                case 205:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_TIPOS_ETIQUETAS");
                    break;
                case 206:
                    masterFile = ConfigurationManager.AppSettings.Get("API_MAESTRO_TIPOS_AJUSTES");
                    break;
            }
            return masterFile;
        }
    }
}

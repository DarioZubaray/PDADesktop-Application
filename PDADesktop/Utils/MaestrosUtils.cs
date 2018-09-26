using log4net;
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace PDADesktop.Utils
{
    class MaestrosUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public static void crearArchivoPAS()
        {
            string separador = "*eof*";
            string publicUbicart = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
            string publicUbicartExtended = TextUtils.ExpandEnviromentVariable(publicUbicart);
            string ubicartFileName = TextUtils.getAAAttributes(Model.ArchivoActividad.UBICART).nombreArchivo;
            logger.Debug("Leyendo archivo: " + ubicartFileName);
            string ubicartContent = FileUtils.ReadFile(publicUbicartExtended + "/" + ubicartFileName);
            if(ubicartContent.IndexOf(separador) != -1)
            {
                String[] resultadoPartes = Regex.Split(ubicartContent, "\\*eof\\*");

                ubicartContent = resultadoPartes[0];
                logger.Debug("Sobreescribiendo: " + ubicartFileName);
                FileUtils.WriteFile(publicUbicartExtended + "/" + ubicartFileName, ubicartContent + "\r\n");

                for (int i = 1; i < resultadoPartes.Length; i++)
                {
                    string parteActual = resultadoPartes[i];
                    string nombrePas = parteActual.Split(':')[0].Trim() + ".pas";
                    string contenido = parteActual.Split(':')[1].Trim();
                    string rutaPas = publicUbicartExtended + "/" + nombrePas;
                    logger.Debug("Guardando archivo pas: " + rutaPas);
                    FileUtils.WriteFile(rutaPas, contenido + "\r\n");
                }
            }
        }

        public static void crearArchivosPedidos()
        {
            string separador = "*eof*";
            string publicPedidos = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
            string publicPedidosExtended = TextUtils.ExpandEnviromentVariable(publicPedidos);
            string pedidosFileName = TextUtils.getAAAttributes(Model.ArchivoActividad.PEDIDOS).nombreArchivo;
            logger.Debug("Leyendo archivo pedidos: " + publicPedidosExtended + "/" + pedidosFileName);
            string pedidosContent = FileUtils.ReadFile(publicPedidosExtended + "/" + pedidosFileName);
            if (pedidosContent.Substring(pedidosContent.Length - 5).Equals(separador))
            {
                //Si el último campo es vacío, agrego un espacio para que lo reconozca el split
                pedidosContent += " ";
            }
            String[] resultadoPartes = Regex.Split(pedidosContent, "\\*eof\\*");
            pedidosContent = resultadoPartes[0];
            FileUtils.WriteFile(publicPedidosExtended + "/" + pedidosFileName, pedidosContent + "\r\n");

            crearArchivoDePEDIDOS(publicPedidosExtended, "LPEDIDOS.DAT", resultadoPartes[1]);
            crearArchivoDePEDIDOS(publicPedidosExtended, "APEDIDOS.DAT", resultadoPartes[2]);
            crearArchivoDePEDIDOS(publicPedidosExtended, "EPEDIDOS.DAT", resultadoPartes[3]);
            crearArchivoDePEDIDOS(publicPedidosExtended, "RPEDIDOS.DAT", resultadoPartes[4]);
        }

        private static void crearArchivoDePEDIDOS(string rutaArchivo, string nombreArchivo, string texto)
        {
            string contenido = texto.Trim();
            logger.Debug("Guardando archivo pedidos: " + rutaArchivo + "/" + nombreArchivo);
            FileUtils.WriteFile(rutaArchivo + "/" + nombreArchivo, contenido + "\r\n");
        }
    }
}

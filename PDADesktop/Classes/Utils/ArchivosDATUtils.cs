using log4net;
using PDADesktop.Classes;
using PDADesktop.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDADesktop.Utils
{
    public static class ArchivosDATUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static ArchivoActividad GetArchivoActividadByIdActividad(int idActividad)
        {
            ArchivoActividad archivoActividad;
            switch (idActividad)
            {
                case 101:
                    archivoActividad = ArchivoActividad.CTRUBIC;
                    break;
                case 102:
                    archivoActividad = ArchivoActividad.CTRSUBIC;
                    break;
                case 103:
                    archivoActividad = ArchivoActividad.AJUTES;
                    break;
                case 104:
                    archivoActividad = ArchivoActividad.RECEP;
                    break;
                case 105:
                    archivoActividad = ArchivoActividad.ETIQ;
                    break;
                case 201:
                    archivoActividad = ArchivoActividad.ART;
                    break;
                case 202:
                    archivoActividad = ArchivoActividad.UBIC;
                    break;
                case 203:
                    archivoActividad = ArchivoActividad.UBICART;
                    break;
                case 204:
                    archivoActividad = ArchivoActividad.PEDIDOS;
                    break;
                case 205:
                    archivoActividad = ArchivoActividad.TETIQ;
                    break;
                case 206:
                    archivoActividad = ArchivoActividad.TAJUS;
                    break;
                case 207:
                    archivoActividad = ArchivoActividad.PROVEED;
                    break;
                case 208:
                    archivoActividad = ArchivoActividad.PROVART;
                    break;
                case 209:
                    archivoActividad = ArchivoActividad.MOTIDEV;
                    break;
                default:
                    throw new Exception("No se puede obtener la actividad " + idActividad);
            }
            return archivoActividad;
        }

        public static ArchivoActividadAttributes GetAAAttributes(this ArchivoActividad value)
        {
            var enumType = value.GetType();
            var enumName = Enum.GetName(enumType, value);
            var archiveAttribute = enumType.GetField(enumName).GetCustomAttributes(false).OfType<ArchivoActividadAttributes>().SingleOrDefault();
            return archiveAttribute as ArchivoActividadAttributes;
        }

        public static string GetDataFileNameAndExtensionByIdActividad(int idActividad)
        {
            return "/" + GetDataFileNameByIdActividad(idActividad) + ".DAT";
        }

        public static string GetDataFileNameByIdActividad(int idActividad)
        {
            string dataFileName = null;
            switch (idActividad)
            {
                case 101:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.CONTROL_PRECIOS_CON_UBICACIONES);
                    break;
                case 102:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.CONTROL_PRECIOS_SIN_UBICACIONES);
                    break;
                case 103:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.AJUSTES);
                    break;
                case 104:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.RECEPCIONES);
                    break;
                case 105:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.IMPRESION_ETIQUETAS);
                    break;
                case 201:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.ARTICULOS);
                    break;
                case 202:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.UBICACIONES);
                    break;
                case 203:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.UBICACION_ARTICULOS);
                    break;
                case 204:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.PEDIDOS);
                    break;
                case 205:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.TIPOS_ETIQUETAS);
                    break;
                case 206:
                    dataFileName = ConfigurationManager.AppSettings.Get(Constants.TIPOS_AJUSTES);
                    break;
            }
            return dataFileName;
        }

        public static void crearArchivoPAS()
        {
            string separador = "*eof*";
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string publicUbicart = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string publicUbicartExtended = TextUtils.ExpandEnviromentVariable(publicUbicart);
            string ubicartFileName = GetAAAttributes(Model.ArchivoActividad.UBICART).nombreArchivo;
            logger.Debug("Leyendo archivo: " + ubicartFileName);
            string ubicartContent = FileUtils.ReadFile(publicUbicartExtended + "/" + ubicartFileName);
            if(ubicartContent.IndexOf(separador) != -1)
            {
                String[] resultadoPartes = Regex.Split(ubicartContent, "\\*eof\\*");

                ubicartContent = resultadoPartes[0];
                logger.Debug("Sobreescribiendo: " + ubicartFileName);
                FileUtils.WriteFile(publicUbicartExtended + "/" + ubicartFileName, ubicartContent);

                for (int i = 1; i < resultadoPartes.Length; i++)
                {
                    string parteActual = resultadoPartes[i];
                    string nombrePas = parteActual.Split(':')[0].Trim() + ".pas";
                    string contenido = parteActual.Split(':')[1].Trim();
                    string rutaPas = publicUbicartExtended + "/" + nombrePas;
                    logger.Debug("Guardando archivo pas: " + rutaPas);
                    FileUtils.WriteFile(rutaPas, contenido + "\r\n");
                    App.Instance.deviceHandler.CopyPublicDataFileToDevice(deviceRelativePathData, "/" + nombrePas);
                }
            }
        }

        public static void crearArchivosPedidos()
        {
            string separador = "*eof*";
            string publicPedidos = ConfigurationManager.AppSettings.Get(Constants.CLIENT_PATH_DATA);
            string publicPedidosExtended = TextUtils.ExpandEnviromentVariable(publicPedidos);
            string pedidosFileName = GetAAAttributes(Model.ArchivoActividad.PEDIDOS).nombreArchivo;
            logger.Debug("Leyendo archivo pedidos: " + publicPedidosExtended + "/" + pedidosFileName);
            string pedidosContent = FileUtils.ReadFile(publicPedidosExtended + "/" + pedidosFileName);
            if (pedidosContent.Substring(pedidosContent.Length - 5).Equals(separador))
            {
                //Si el último campo es vacío, agrego un espacio para que lo reconozca el split
                pedidosContent += " ";
            }
            String[] resultadoPartes = Regex.Split(pedidosContent, "\\*eof\\*");
            pedidosContent = resultadoPartes[0];
            logger.Debug("sobreescribiendo: " + pedidosFileName);
            FileUtils.WriteFile(publicPedidosExtended + "/" + pedidosFileName, pedidosContent);

            crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.RPEDIDOS, resultadoPartes[1]);
            crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.APEDIDOS, resultadoPartes[2]);
            crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.EPEDIDOS, resultadoPartes[3]);
            crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.RPEDIDOS, resultadoPartes[4]);
        }

        private static void crearMoverArchivoDePEDIDOS(string rutaArchivo, string filenameAndExtension, string texto)
        {
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string contenido = texto.Trim();
            logger.Debug("Guardando archivo pedidos: " + rutaArchivo + "/" + filenameAndExtension);
            FileUtils.WriteFile(rutaArchivo + "/" + filenameAndExtension, contenido + "\r\n");
            App.Instance.deviceHandler.CopyPublicDataFileToDevice(deviceRelativePathData, filenameAndExtension);
        }
    }
}

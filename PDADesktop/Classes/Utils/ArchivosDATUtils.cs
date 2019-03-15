using log4net;
using PDADesktop.Model;
using System;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDADesktop.Classes.Utils
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
            string dataFileName = GetDataFileNameByIdActividad(idActividad);
            return FileUtils.WrapSlashAndDATExtension(dataFileName);
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

        public static void createPASFile()
        {
            string separador = "*eof*";
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string publicUbicart = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicUbicartExtended = TextUtils.ExpandEnviromentVariable(publicUbicart);
            string ubicartFileName = GetAAAttributes(Model.ArchivoActividad.UBICART).nombreArchivo;
            logger.Debug("Leyendo archivo: " + ubicartFileName);
            string ubicarPath = publicUbicartExtended + FileUtils.PrependSlash(ubicartFileName);
            string ubicartContent = FileUtils.ReadFile(ubicarPath);
            if(ubicartContent.IndexOf(separador) != -1)
            {
                String[] resultadoPartes = Regex.Split(ubicartContent, "\\*eof\\*");

                ubicartContent = resultadoPartes[0];
                logger.Debug("Sobreescribiendo: " + ubicartFileName);
                FileUtils.WriteFile(ubicarPath, ubicartContent);

                for (int i = 1; i < resultadoPartes.Length; i++)
                {
                    string parteActual = resultadoPartes[i];
                    string nombrePas = parteActual.Split(':')[0].Trim() + ".pas";
                    string contenido = parteActual.Split(':')[1].Trim();
                    string rutaPas = publicUbicartExtended + FileUtils.PrependSlash(nombrePas);
                    logger.Debug("Guardando archivo pas: " + rutaPas);
                    FileUtils.WriteFile(rutaPas, contenido + "\r\n");
                    App.Instance.deviceHandler.CopyPublicDataFileToDevice(deviceRelativePathData, FileUtils.PrependSlash(nombrePas));
                }
            }
        }

        public static void createOrdersFiles()
        {
            string separador = "*eof*";
            string publicPedidos = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string publicPedidosExtended = TextUtils.ExpandEnviromentVariable(publicPedidos);
            string pedidosFileName = GetAAAttributes(Model.ArchivoActividad.PEDIDOS).nombreArchivo;
            logger.Debug("Leyendo archivo pedidos: " + publicPedidosExtended + FileUtils.PrependSlash(pedidosFileName));
            string pedidosContent = FileUtils.ReadFile(publicPedidosExtended + FileUtils.PrependSlash(pedidosFileName));
            logger.Debug("Contenido de Pedidos: " + pedidosContent);
            if(pedidosContent != null && pedidosContent.Length > 0)
            {
                if (pedidosContent.EndsWith(separador))
                {
                    //Si el último campo es vacío, agrego un espacio para que lo reconozca el split
                    pedidosContent += " ";
                }
                int totalArchivos = Regex.Matches(pedidosContent, ToLiteral(separador)).Count;
                logger.Debug("Total de partes de pedidos: " + totalArchivos);

                String[] resultadoPartes = Regex.Split(pedidosContent, ToLiteral(separador));
                int parteActual = 0;
                pedidosContent = resultadoPartes[parteActual];
                parteActual++;
                logger.Debug("sobreescribiendo: " + pedidosFileName);
                FileUtils.WriteFile(publicPedidosExtended + FileUtils.PrependSlash(pedidosFileName), pedidosContent);

                if (totalArchivos >= parteActual)
                {
                    crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.LPEDIDOS, resultadoPartes[parteActual]);
                    parteActual++;
                }
                if (totalArchivos >= parteActual)
                {
                    crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.APEDIDOS, resultadoPartes[parteActual]);
                    parteActual++;
                }
                if (totalArchivos >= parteActual)
                {
                    crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.EPEDIDOS, resultadoPartes[parteActual]);
                    parteActual++;
                }
                if (totalArchivos >= parteActual)
                {
                    crearMoverArchivoDePEDIDOS(publicPedidosExtended, Constants.RPEDIDOS, resultadoPartes[parteActual]);
                    parteActual++;
                }
            }
        }

        private static string ToLiteral(string input)
        {
            return input.Replace("*", "\\*");
        }

        private static void crearMoverArchivoDePEDIDOS(string rutaArchivo, string filename, string texto)
        {
            string deviceRelativePathData = ConfigurationManager.AppSettings.Get(Constants.DEVICE_RELPATH_DATA);
            string contenido = texto.Trim();
            string slashFilenameAndExtension = FileUtils.WrapSlashAndDATExtension(filename);
            logger.Debug("Guardando archivo pedidos: " + rutaArchivo + slashFilenameAndExtension);
            FileUtils.WriteFile(rutaArchivo + slashFilenameAndExtension, contenido + "\r\n");
            App.Instance.deviceHandler.CopyPublicDataFileToDevice(deviceRelativePathData, slashFilenameAndExtension);
        }

        private static void OverrideDATFileinPublic(string newContents, string filename)
        {
            string pathFile = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);
            string pathFileExtended = TextUtils.ExpandEnviromentVariable(pathFile);
            FileUtils.WriteFile(pathFileExtended + filename, newContents);
        }

        public static void OverrideCTRUBICDATinPublic(string newContents)
        {
            string filename = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_CONTROL_PRECIO);
            OverrideDATFileinPublic(newContents, filename);
        }

        public static void OverrideAJUSTESDATinPublic(string newContents)
        {
            string filename = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_AJUSTES);
            OverrideDATFileinPublic(newContents, filename);
        }

        public static void OverrideRECEPDATinPublic(string newContents)
        {
            string filename = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_RECEPCIONES);
            OverrideDATFileinPublic(newContents, filename);
        }

        public static void OverrideETIQDATinPublic(string newContents)
        {
            string filename = ConfigurationManager.AppSettings.Get(Constants.DAT_FILE_ETIQUETAS);
            OverrideDATFileinPublic(newContents, filename);
        }
    }
}

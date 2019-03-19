using log4net;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace PDADesktop.Classes.Utils
{
    public static class TextUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ExpandEnviromentVariable(string source)
        {
            return Environment.ExpandEnvironmentVariables(source);
        }

        public static string ParsePriceControlDAT2JsonStr(string source)
        {
            StringBuilder controlPrecioJSON = new StringBuilder();
            controlPrecioJSON.Append("[");
            String[] lineas = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int enBasecero = -1;
            int tamanioArray = lineas.Length + enBasecero;
            for(int i = 0; i <= tamanioArray; i++)
            {
                String controlPrecio = lineas[i];
                if(!controlPrecio.Equals("") && controlPrecio.Contains("|"))
                {
                    if (i > 0 && i < tamanioArray)
                    {
                        controlPrecioJSON.Append(",");
                    }
                    String[] item = controlPrecio.Split('|');
                    controlPrecioJSON.Append("{\"EAN\": \"" + item[0] + "\"");
                    controlPrecioJSON.Append(", \"fecha\": " + item[1]);
                    controlPrecioJSON.Append(", \"TipoLectura\": " + item[2]);
                    controlPrecioJSON.Append(", \"Pasillo\": \"" + item[3] + "\"");
                    controlPrecioJSON.Append(", \"ControlUbicacion\": " + item[4]);
                    controlPrecioJSON.Append(", \"IDEtiqueta\": \"" + item[5] + "\"");
                    controlPrecioJSON.Append(", \"CantidadEtiquetas\": " + item[6]);
                    controlPrecioJSON.Append(", \"AlertaStock\": " + item[7]);
                    controlPrecioJSON.Append(", \"NumeroSecuencia\": \"" + item[8] + "\" }");
                }
                else
                {
                    if (i >= tamanioArray)
                    {
                        controlPrecioJSON.Append("]");
                    }
                    logger.Info("VA - parseControlPreciosDAT2Json linea skipeada: " + controlPrecio);
                }
            }
            return controlPrecioJSON.ToString();
        }

        public static string ParseAdjustmentDAT2JsonStr(string source)
        {
            StringBuilder ajusteJSON = new StringBuilder();
            ajusteJSON.Append("[");
            String[] lineas = source.Split( new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int enBaseCero = -1;
            int tamanioArray = lineas.Length + enBaseCero;
            for (int i = 0; i <= tamanioArray; i++)
            {
                String ajuste = lineas[i];
                if (ajuste.Contains("|"))
                {
                    String[] item = ajuste.Split('|');
                    ajusteJSON.Append("{\"ean\": " + item[0]);
                    ajusteJSON.Append(", \"fechaAjuste\": " + item[1]);
                    ajusteJSON.Append(", \"motivo\": \"" + item[2] + "\"");
                    ajusteJSON.Append(", \"perfilGenesix\": \"" + item[3] + "\"");
                    ajusteJSON.Append(", \"cantidad\": " + item[4]);
                    ajusteJSON.Append(", \"claveAjuste\": \"" + item[5] + "\"}");
                    if (i < tamanioArray)
                    {
                        ajusteJSON.Append(",");
                    }
                    else
                    {
                        ajusteJSON.Append("]");
                    }
                }
                else
                {
                    if (i >= tamanioArray)
                    {
                        ajusteJSON.Append("]");
                    }
                    logger.Info("VA - parseAjusteDAT2Json linea skipeada: " + ajuste);
                }
            }
            return ajusteJSON.ToString();
        }

        public static string ParseReceptionsDAT2JsonStr(string source)
        {
            StringBuilder recepcionesJSON = new StringBuilder();
            recepcionesJSON.Append("[");
            String[] lineas = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int enBasecero = -1;
            int tamanioArray = lineas.Length + enBasecero;
            for (int i = 0; i <= tamanioArray; i++)
            {
                String recepciones = lineas[i];
                if (!recepciones.Equals("") && recepciones.Contains("|"))
                {
                    if (i > 0 && i <= tamanioArray)
                    {
                        recepcionesJSON.Append(",");
                    }
                    String[] item = recepciones.Split('|');
                    recepcionesJSON.Append("{\"recepcion\": {");
                    recepcionesJSON.Append("\"fechaRecep\": \"" + item[0] + "\"");
                    recepcionesJSON.Append(", \"numeroPedido\": " + item[1]);
                    recepcionesJSON.Append(", \"numeroProveedor\": " + item[2]);
                    //R - campo 3 sin uso
                    recepcionesJSON.Append(", \"sucursalRemito\": " + item[4]);
                    recepcionesJSON.Append(", \"numeroRemito\": \"" + item[5] + "\"");
                    recepcionesJSON.Append(", \"fechaRem\": \"" + item[6] + "\"");
                    recepcionesJSON.Append(", \"descripcionProveedor\": \"" + item[10] + "\" }");
                    // Clave - campo 9 sin uso
                    recepcionesJSON.Append(", \"EAN\": " + item[7]);
                    recepcionesJSON.Append(", \"unidadesRecibidas\": " + item[8] + " }");
                }
            }
            recepcionesJSON.Append("]");
            return recepcionesJSON.ToString();
        }

        public static string ParseLabelsDAT2JsonStr(string source)
        {
            StringBuilder etiquetaJSON = new StringBuilder();
            etiquetaJSON.Append("[");
            String[] lineas = source.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int enBasecero = -1;
            int tamanioArray = lineas.Length + enBasecero;
            for (int i = 0; i <= tamanioArray; i++)
            {
                String etiquetas = lineas[i];
                if (!etiquetas.Equals("") && etiquetas.Contains("|"))
                {
                    if (i > 0 && i < tamanioArray)
                    {
                        etiquetaJSON.Append(",");
                    }
                    String[] item = etiquetas.Split('|');
                    etiquetaJSON.Append("{\"EAN\": \"" + item[0] + "\"");
                    etiquetaJSON.Append(", \"Fecha\": \"" + item[1] + "\"");
                    etiquetaJSON.Append(", \"CodigoEtiqueta\": \"" + item[2] + "\" }");
                }
                else
                {
                    if (i >= tamanioArray)
                    {
                        etiquetaJSON.Append("]");
                    }
                    logger.Info("VA - parseEtiquetasDAT2Json linea skipeada: " + etiquetas);
                }
            }
            return etiquetaJSON.ToString();
        }

        public static string ParseCollectionToAdjustmentDAT(ObservableCollection<Ajustes> ajustes)
        {
            StringBuilder adjustmentContent = new StringBuilder();
            foreach(Ajustes ajuste in ajustes)
            {
                adjustmentContent.Append(ajuste.ean).Append("|");
                adjustmentContent.Append(ajuste.fechaAjuste).Append("|");
                adjustmentContent.Append(ajuste.motivo).Append("|");
                adjustmentContent.Append(ajuste.perfilGenesix).Append("|");
                adjustmentContent.Append(ajuste.cantidad).Append("|");
                adjustmentContent.Append(ajuste.claveAjuste).Append("\r\n");
            }
            return adjustmentContent.ToString();
        }
        public static string ParseListAccion2String(List<Accion> acciones)
        {
            StringBuilder sb = new StringBuilder("[");
            for (int i = 0; i < acciones.Count; i++)
            {
                sb.Append(acciones[i].idAccion.ToString());
                if (i < acciones.Count - 1)
                {
                    sb.Append(", ");
                }
                else
                {
                    sb.Append("]");
                }
            }
            return sb.ToString();
        }

        public static ArchivoActividadAttributes GetAAAttributes(this ArchivoActividad value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var aa = enumType.GetField(name).GetCustomAttributes(false).OfType<ArchivoActividadAttributes>().SingleOrDefault();
            return aa as ArchivoActividadAttributes;
        }

        public static string RemoveQuotesMarks(string defaultContent)
        {
            if (defaultContent != null)
            {
                if (defaultContent.Contains("\""))
                {
                    defaultContent = defaultContent.Replace("\"", "");
                }
            }
            return defaultContent;
        }

        public static string GetVersionFromDefaultDat(string defaultContent)
        {
            return GetIndexContentByDefaultDatPosition(defaultContent, DeviceMainData.POSITION_VERSION);
        }

        public static string GetStoreIdFromDefaultDat(string defaultContent)
        {
            return GetIndexContentByDefaultDatPosition(defaultContent, DeviceMainData.POSITION_SUCURSAL);
        }

        public static string GetSynchronizationDateFromDefaultDat(string defaultContent)
        {
            return GetIndexContentByDefaultDatPosition(defaultContent, DeviceMainData.POSITION_FECHA_SINCO);
        }

        public static string GetSynchronizationStateFromDefaultDat(string defaultContent)
        {
            return GetIndexContentByDefaultDatPosition(defaultContent, DeviceMainData.POSITION_ESTADO_SINCRO);
        }

        private static string GetIndexContentByDefaultDatPosition(string defaultContent, int indiceDefault)
        {
            String[] defaultArray = defaultContent.Split('|');
            if (defaultArray.Length == DeviceMainData.TOTAL_POSITION_ONE_BASE)
            {
                return defaultArray[indiceDefault];
            }
            else
            {
                return null;
            }
        }

        public static bool CompareStoreId(string bf1, string bf2)
        {
            if(bf1 != null && bf2 != null)
            {
                return bf1.Trim().Equals(bf2.Trim(), StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                return false;
            }
        }

        public static string BuildDefaultContent(string previousContent, string newContent, int position)
        {
            String[] defaultArray = previousContent.Split('|');
            if (defaultArray.Length == DeviceMainData.TOTAL_POSITION_ONE_BASE)
            {
                defaultArray[position] = newContent;
            }
            return String.Join("|", defaultArray);
        }
    }
}

using log4net;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PDADesktop.Classes.Utils
{
    public static class TextUtils
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string ExpandEnviromentVariable(string source)
        {
            return Environment.ExpandEnvironmentVariables(source);
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

        public static string ParseCollectionToAdjustmentDAT(ObservableCollection<Ajustes> ajustes)
        {
            StringBuilder adjustmentContent = new StringBuilder();
            foreach(Ajustes ajuste in ajustes)
            {
                adjustmentContent.Append(ajuste.ean).Append("|");
                adjustmentContent.Append(ajuste.fechaAjuste).Append("|");
                adjustmentContent.Append(ajuste.motivo).Append("|");
                adjustmentContent.Append(ajuste.cantidad).Append("|");
                adjustmentContent.Append(ajuste.perfilGenesix).Append("|");
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

        public static ArchivoActividadAttributes getAAAttributes(this ArchivoActividad value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            var aa = enumType.GetField(name).GetCustomAttributes(false).OfType<ArchivoActividadAttributes>().SingleOrDefault();
            return aa as ArchivoActividadAttributes;
        }

        public static string getVersionFromDefaultDat(string defaultContent)
        {
            String[] defaultArray = defaultContent.Split('|');
            if(defaultArray.Length == 5)
            {
                int indiceVersion = 4;
                return defaultArray[indiceVersion];
            }
            else
            {
                return null;
            }
        }
    }
}

using log4net;
using System;
using System.Text;

namespace PDADesktop.Utils
{
    class TextUtils
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
                    logger.Info("VA - parseAjusteDAT2Json linea skipeada: " + ajuste);
                }
            }
            return ajusteJSON.ToString();
        }
    }
}

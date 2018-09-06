using log4net;
using PDADesktop.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PDADesktop.Classes
{
    class HttpWebClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static List<Sincronizacion> GetHttpWebSincronizacion(string url)
        {
            List<Sincronizacion> sincronizaciones = null;
            var client = new System.Net.WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");

            try
            {
                var response = client.DownloadString(url);
                sincronizaciones = JsonConvert.DeserializeObject<List<Sincronizacion>>(response);
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
            }
            return sincronizaciones;
        }

        public static Boolean getHttpWebServerConexionStatus(string url)
        {
            Boolean conexionStatus = false;
            var client = new System.Net.WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            try
            {
                var response = client.DownloadString(url);
                logger.Debug("response: " + response);
                conexionStatus = response.Length > 0 ? true : false;
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                conexionStatus = false;
            }
            return conexionStatus;
        }
    }
}

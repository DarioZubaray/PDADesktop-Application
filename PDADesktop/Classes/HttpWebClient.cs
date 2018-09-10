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
using System.Windows;
using System.Configuration;

namespace PDADesktop.Classes
{
    class HttpWebClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Boolean getHttpWebServerConexionStatus(string url)
        {
            Boolean conexionStatus = false;
            string response = sendHttpRequest(url);
            if (response != null)
            {
                conexionStatus = response.Length > 0 ? true : false;
            }
            return conexionStatus;
        }

        public static int GetIdLoteActual(string url, string sucursal)
        {
            int idLote = 0;
            string response = sendHttpRequest(url + "?idSucursal=" + sucursal);
            if(response != null)
            {
                if(response.Contains("\""))
                {
                    response = response.Replace("\"", "");
                }
                idLote = Convert.ToInt32(response);
            }
            return idLote;
        }

        public static List<Sincronizacion> GetHttpWebSincronizacion(string url)
        {
            List<Sincronizacion> sincronizaciones = null;

            string response = sendHttpRequest(url);
            if(response != null)
            {
                sincronizaciones = JsonConvert.DeserializeObject<List<Sincronizacion>>(response);
            }

            return sincronizaciones;
        }

        public static List<Sincronizacion> GetHttpWebSincronizacion(string url, string idSucursal, string idLote)
        {
            List<Sincronizacion> sincronizaciones = null;

            string response = sendHttpRequest(url + "?idSucursal=" + idSucursal + "&idLote=" + idLote);
            if (response != null)
            {
                sincronizaciones = JsonConvert.DeserializeObject<List<Sincronizacion>>(response);
            }

            return sincronizaciones;
        }

        public static string sendHttpRequest(String url)
        {
            string response = null;
            var client = new System.Net.WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string ipServer = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                response = client.DownloadString(ipServer + url);
                logger.Debug("response: " + response);
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                string message = e.GetType() + " - " + e.Message;
                string caption = "Error de comunicacion con PDA Express Server";
                MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return response;
        }
    }
}

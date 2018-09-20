using log4net;
using PDADesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;
using PDADesktop.Utils;

namespace PDADesktop.Classes
{
    class HttpWebClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string sendHttpRequest(string url)
        {
            string response = null;
            var client = new System.Net.WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string ipServer = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + ipServer + url);
                response = client.DownloadString(ipServer + url);
                //logger.Debug("response: " + response);
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

        public static string DownloadMasterFiles(string url)
        {
            string response = null;
            var client = new System.Net.WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string ipServer = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + ipServer + url);
                //Si la path no existe se rompe en mil pedacitos =/
                string destino = @"C:/dev/PDADesktop/Maestros";
                FileUtils.VerifyFoldersOrCreate(destino);
                client.DownloadFile(ipServer + url, destino + "/MaestroArticulo.DAT");
                //logger.Debug("response: " + response);
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

        public static Boolean getHttpWebServerConexionStatus()
        {
            Boolean conexionStatus = false;
            string urlServerStatus = ConfigurationManager.AppSettings.Get("API_SERVER_CONEXION_STATUS");
            string response = sendHttpRequest(urlServerStatus);
            if (response != null)
            {
                conexionStatus = response.Length > 0 ? true : false;
            }
            return conexionStatus;
        }

        public static int GetIdLoteActual(string sucursal)
        {
            int idLote = 0;
            string urlIdLoteActual = ConfigurationManager.AppSettings.Get("API_SYNC_ID_LOTE");
            string url = String.Format("{0}?idSucursal={1}", urlIdLoteActual, sucursal);
            string response = sendHttpRequest(url);
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

        public static List<Sincronizacion> GetHttpWebSincronizacion(string url, string idSucursal, string idLote)
        {
            List<Sincronizacion> sincronizaciones = null;
            string endpoint = String.Format("{0}?idSucursal={1}&idLote={2}", url, idSucursal, idLote);
            string response = sendHttpRequest(endpoint);
            if (response != null)
            {
                sincronizaciones = JsonConvert.DeserializeObject<List<Sincronizacion>>(response);
            }

            return sincronizaciones;
        }

        public static List<string> GetTiposDeAjustes()
        {
            List<string> tiposAjustes = null;
            string urlTiposAjustes = ConfigurationManager.AppSettings.Get("API_GET_TIPOS_AJUSTES");
            string response = sendHttpRequest(urlTiposAjustes);
            if (response != null)
            {
                tiposAjustes = JsonConvert.DeserializeObject<List<string>>(response);
            }
            return tiposAjustes;
        }

        internal static bool CheckRecepcionesInformadas(string idSincronizacion)
        {
            bool recepcionesInformadas = false;
            string url = ConfigurationManager.AppSettings.Get("API_BUSCAR_RECEPCIONES_INFORMADAS");
            string response = sendHttpRequest(String.Format("{0}?idSincronizacion={1}", url, idSincronizacion));
            if (response != null)
            {
                recepcionesInformadas = response.Equals("\"1\"") ? true : false;
            }
            return recepcionesInformadas;
        }

        internal static string buscarMaestroArt(string idSucursal)
        {
            string url = ConfigurationManager.AppSettings.Get("API_MAESTRO_ARTICULOS");
            string response = DownloadMasterFiles(String.Format("{0}?idSucursal={1}", url, idSucursal));
            return response;
        }
    }
}

using log4net;
using PDADesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;
using PDADesktop.Utils;
using System.Net;
using System.IO;

namespace PDADesktop.Classes
{
    class HttpWebClient
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string sendHttpGetRequest(string urlPath)
        {
            string response = null;
            var client = new WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + urlAuthority + urlPath);
                response = client.DownloadString(urlAuthority + urlPath);
                if(response.Length < 100)
                {
                    logger.Debug("response: " + response);
                }
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                showErrorMessage(e);
            }
            return response;
        }

        public static string sendHttpPostRequest(string urlPath, string jsonBody)
        {
            string result = null;
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(urlAuthority + urlPath);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonBody);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }

        public static void showErrorMessage(Exception e)
        {
            string message = e.GetType() + " - " + e.Message;
            string caption = "Error de comunicacion con PDA Express Server";
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static bool DownloadMasterFiles(string urlPath, string masterFile)
        {
            var client = new WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + urlAuthority + urlPath);
                string destino = ConfigurationManager.AppSettings.Get("CLIENT_PATH_DATA");
                destino = TextUtils.ExpandEnviromentVariable(destino);
                FileUtils.VerifyFoldersOrCreate(destino);
                logger.Debug("Descargando en: " + destino + String.Format("/{0}.DAT", masterFile));
                client.DownloadFile(urlAuthority + urlPath, destino + String.Format("/{0}.DAT", masterFile));
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                showErrorMessage(e);
                return false;
            }
        }

        public static Boolean getHttpWebServerConexionStatus()
        {
            Boolean conexionStatus = false;
            string urlServerStatus = ConfigurationManager.AppSettings.Get("API_SERVER_CONEXION_STATUS");
            string response = sendHttpGetRequest(urlServerStatus);
            if (response != null)
            {
                conexionStatus = response.Length > 0 ? true : false;
            }
            return conexionStatus;
        }

        public static int GetIdLoteActual(string idSucursal)
        {
            int idLote = 0;
            string urlPath = ConfigurationManager.AppSettings.Get("API_SYNC_ID_LOTE");
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}", urlPath, idSucursal);
            string response = sendHttpGetRequest(urlPath_urlQuery);
            if(response != null && !response.Equals("null"))
            {
                if(response.Contains("\""))
                {
                    response = response.Replace("\"", "");
                }
                idLote = Convert.ToInt32(response);
            }
            return idLote;
        }

        public static List<Sincronizacion> GetHttpWebSincronizacion(string urlPath, string idSucursal, string idLote)
        {
            List<Sincronizacion> sincronizaciones = null;
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}&idLote={2}", urlPath, idSucursal, idLote);
            string response = sendHttpGetRequest(urlPath_urlQuery);
            if (response != null)
            {
                sincronizaciones = JsonConvert.DeserializeObject<List<Sincronizacion>>(response);
            }

            return sincronizaciones;
        }

        public static List<string> GetTiposDeAjustes()
        {
            List<string> tiposAjustes = null;
            string urlPath = ConfigurationManager.AppSettings.Get("API_GET_TIPOS_AJUSTES");
            string response = sendHttpGetRequest(urlPath);
            if (response != null)
            {
                tiposAjustes = JsonConvert.DeserializeObject<List<string>>(response);
            }
            return tiposAjustes;
        }

        internal static bool CheckRecepcionesInformadas(string idSincronizacion)
        {
            bool recepcionesInformadas = false;
            string urlPath = ConfigurationManager.AppSettings.Get("API_BUSCAR_RECEPCIONES_INFORMADAS");
            string urlPath_urlQuery = String.Format("{0}?idSincronizacion={1}", urlPath, idSincronizacion);
            string response = sendHttpGetRequest(urlPath_urlQuery);
            if (response != null)
            {
                recepcionesInformadas = response.Equals("\"1\"") ? true : false;
            }
            return recepcionesInformadas;
        }

        internal static bool buscarMaestrosDAT(int idActividad, string idSucursal)
        {
            string masterFile = MaestrosUtils.GetMasterFileName(idActividad);
            string urlPath = ConfigurationManager.AppSettings.Get("API_MAESTRO_URLPATH");
            urlPath = String.Format(urlPath, masterFile);
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}", urlPath, idSucursal);
            return DownloadMasterFiles(urlPath_urlQuery, masterFile);
        }
    }
}

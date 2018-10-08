using log4net;
using PDADesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;

namespace PDADesktop.Classes.Utils
{
    class HttpWebClientUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Request
        public static string SendHttpGetRequest(string urlPath)
        {
            string response = null;
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + urlAuthority + urlPath);
                using (var client = new PDAWebClient(20000))
                {
                    response = client.DownloadString(urlAuthority + urlPath);
                    if (response.Length < 100)
                    {
                        logger.Debug("response: " + response);
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                ShowErrorMessage(e);
            }
            return response;
        }

        public static string SendHttpPostRequest(string urlPath, string jsonBody)
        {
            string result = null;
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlAuthority + urlPath);
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

        public static string SendFileHttpRequest(string filePath, string url)
        {
            WebResponse response = null;
            try
            {
                filePath = TextUtils.ExpandEnviromentVariable(filePath);
                string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
                string sWebAddress = urlAuthority + url;

                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(sWebAddress);
                wr.Timeout = 1000 * 15;
                wr.ContentType = "multipart/form-data; boundary=" + boundary;
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Credentials = CredentialCache.DefaultCredentials;
                Stream stream = wr.GetRequestStream();

                stream.Write(boundarybytes, 0, boundarybytes.Length);
                byte[] formitembytes = Encoding.UTF8.GetBytes(filePath);
                stream.Write(formitembytes, 0, formitembytes.Length);
                stream.Write(boundarybytes, 0, boundarybytes.Length);
                string headerTemplate = "Content-Disposition: form-data; name=\"archivo\"; filename=\"{0}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string header = string.Format(headerTemplate, Path.GetFileName(filePath));
                byte[] headerbytes = Encoding.UTF8.GetBytes(header);
                stream.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                    stream.Write(buffer, 0, bytesRead);
                fileStream.Close();

                byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                stream.Write(trailer, 0, trailer.Length);
                stream.Close();

                response = wr.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string responseData = streamReader.ReadToEnd();
                return responseData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
        #endregion

        public static void ShowErrorMessage(Exception e)
        {
            string message = e.GetType() + " - " + e.Message;
            string caption = "Error de comunicacion con PDA Express Server";
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static bool DownloadFileFromServer(string urlPath, string filenameAndExtension, string destino)
        {
            var client = new WebClient();
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
            string urlAuthority = ConfigurationManager.AppSettings.Get("SERVER_HOST_PROTOCOL_IP_PORT");
            try
            {
                logger.Debug("Enviando petición a " + urlAuthority + urlPath);
                destino = TextUtils.ExpandEnviromentVariable(destino);
                FileUtils.VerifyFoldersOrCreate(destino);
                logger.Debug("Descargando en: " + destino + filenameAndExtension);
                client.DownloadFile(urlAuthority + urlPath, destino + filenameAndExtension);
                return true;
            }
            catch (Exception e)
            {
                logger.Error(e.GetType() + " - " + e.Message);
                ShowErrorMessage(e);
                return false;
            }
        }

        public static Boolean GetHttpWebServerConexionStatus()
        {
            Boolean conexionStatus = false;
            string urlServerStatus = ConfigurationManager.AppSettings.Get("API_SERVER_CONEXION_STATUS");
            string response = SendHttpGetRequest(urlServerStatus);
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
            string response = SendHttpGetRequest(urlPath_urlQuery);
            if (response != null && !response.Equals("null"))
            {
                if (response.Contains("\""))
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
            string response = SendHttpGetRequest(urlPath_urlQuery);
            if (response != null)
            {
                sincronizaciones = JsonUtils.GetListSinchronization(response);
            }

            return sincronizaciones;
        }

        public static List<string> GetTiposDeAjustes()
        {
            List<string> tiposAjustes = null;
            string urlPath = ConfigurationManager.AppSettings.Get("API_GET_TIPOS_AJUSTES");
            string response = SendHttpGetRequest(urlPath);
            if (response != null)
            {
                tiposAjustes = JsonUtils.GetListStringOfAdjustment(response);
            }
            return tiposAjustes;
        }

        internal static bool CheckRecepcionesInformadas(string idSincronizacion)
        {
            bool recepcionesInformadas = false;
            string urlPath = ConfigurationManager.AppSettings.Get("API_BUSCAR_RECEPCIONES_INFORMADAS");
            string urlPath_urlQuery = String.Format("{0}?idSincronizacion={1}", urlPath, idSincronizacion);
            string response = SendHttpGetRequest(urlPath_urlQuery);
            if (response != null)
            {
                recepcionesInformadas = response.Equals("\"1\"") ? true : false;
            }
            return recepcionesInformadas;
        }

        internal static bool BuscarMaestrosDAT(int idActividad, string idSucursal)
        {
            string masterFile = ArchivosDATUtils.GetDataFileNameByIdActividad(idActividad);
            string urlPath = ConfigurationManager.AppSettings.Get("API_MAESTRO_URLPATH");
            urlPath = String.Format(urlPath, masterFile);
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}", urlPath, idSucursal);
            string filenameAndExtension = String.Format("/{0}.DAT", masterFile);
            string destinoPublicFolder = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);

            return DownloadFileFromServer(urlPath_urlQuery, filenameAndExtension, destinoPublicFolder);
        }

        public static List<VersionDispositivo> GetInfoVersiones(int dispositivo, Boolean habilitada)
        {
            string urlGetInfoVersiones = ConfigurationManager.AppSettings.Get(Constants.API_GET_INFO_VERSION);
            string queryParams = "?dispositivo={0}&habilitada={1}";
            queryParams = String.Format(queryParams, dispositivo, habilitada);
            var responseInfoVersiones = HttpWebClientUtil.SendHttpGetRequest(urlGetInfoVersiones + queryParams);
            if (responseInfoVersiones != null)
            {
                logger.Debug("respuesta recibida de GetInfoVersiones");
                logger.Debug(responseInfoVersiones);
                List<VersionDispositivo> inforVersiones = JsonUtils.GetVersionDispositivo<VersionDispositivo>(responseInfoVersiones);
                return new List<VersionDispositivo>(inforVersiones);
            }
            else
            {
                return new List<VersionDispositivo>();
            }
        }

        internal static void DownloadDevicePrograms(string idVersionArchivo, string nombre)
        {
            string urlDownloadFile = ConfigurationManager.AppSettings.Get(Constants.API_DOWNLOAD_PROGRAM_FILE);
            string queryParams = "?idVersionArchivo={0}";
            queryParams = String.Format(queryParams, idVersionArchivo);
            
            string destino = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_BIN);
            DownloadFileFromServer(urlDownloadFile+queryParams, FileUtils.PrependSlash(nombre), destino);
        }
    }
}

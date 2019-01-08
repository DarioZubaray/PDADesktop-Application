using log4net;
using PDADesktop.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text;
using PDADesktop.Model.Dto;
using System.Collections.ObjectModel;
using PDADesktop.Model.Portal;

namespace PDADesktop.Classes.Utils
{
    class HttpWebClientUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Requests
        private static string SendHttpGetRequest(string urlPath, string urlAuthority = "PDAExpress")
        {
            string response = null;
            if(urlAuthority.Equals("PDAExpress"))
            {
                urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PDAEXPRESS_SERVER_HOST);
            }


            logger.Debug("Enviando petición a " + urlAuthority + urlPath);
            var clientTimeoutRetry = new PDAWebClient();

            response = clientTimeoutRetry.GetRequest(urlAuthority + urlPath, 150, 3);
            if (response != null && response.Length < 100)
            {
                logger.Debug("response: " + response);
            }
            return response;
        }

        private static string SendHttpPostRequest(string urlPath, string jsonBody, string urlAuthority = "PDAExpress")
        {
            string result = null;
            if (urlAuthority.Equals("PDAExpress"))
            {
                urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PDAEXPRESS_SERVER_HOST);
            }
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(urlAuthority + urlPath);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            try
            {
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
            }
            catch (Exception e)
            {
                logger.Error(e.Message, e);
            }
            return result;
        }

        private static string SendFileHttpRequest(string filePath, string url)
        {
            WebResponse response = null;
            try
            {
                filePath = TextUtils.ExpandEnviromentVariable(filePath);
                string urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PDAEXPRESS_SERVER_HOST);
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

        internal static List<Accion> GetAllActions()
        {
            string urlAcciones = ConfigurationManager.AppSettings.Get(Constants.API_GET_ALL_ACCIONES);
            var responseAcciones = SendHttpGetRequest(urlAcciones);
            if (responseAcciones != null)
            {
                return JsonUtils.GetListAcciones(responseAcciones);
            }
            else
            {
                return null;
            }
        }

        internal static List<Actividad> GetActivitiesByActionId(List<Accion> actions)
        {
            string actionsIds = TextUtils.ParseListAccion2String(actions);
            string jsonBody = "{ \"idAcciones\": " + actionsIds.ToString() + "}";

            var urlActivities = ConfigurationManager.AppSettings.Get(Constants.API_GET_ACTIVIDADES);
            string responseActivities = HttpWebClientUtil.SendHttpPostRequest(urlActivities, jsonBody);
            logger.Debug(responseActivities);
            return JsonUtils.GetListActividades(responseActivities);
        }

        internal static bool DownloadFileFromServer(string urlPath, string filenameAndExtension, string destino, int timeout = 150000)
        {
            string urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PDAEXPRESS_SERVER_HOST);
            var client = new PDAWebClient(urlAuthority + urlPath, timeout);
            string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
            client.Headers.Add("user-agent", userAgent);
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

        internal static Boolean GetHttpWebServerConexionStatus()
        {
            Boolean conexionStatus = false;
            string urlServerStatus = ConfigurationManager.AppSettings.Get(Constants.API_PING);
            string response = SendHttpGetRequest(urlServerStatus);
            if (response != null)
            {
                int index = response.IndexOf("pong");
                conexionStatus = index >= 0 ;
            }
            return conexionStatus;
        }

        internal static Boolean GetHttpWebPortalServerConexionStatus()
        {
            Boolean conexionStatus = false;
            string urlServerStatus = ConfigurationManager.AppSettings.Get(Constants.PORTAL_PING);
            string urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PORTAL_SERVER_HOST);

            string response = SendHttpGetRequest(urlServerStatus, urlAuthority);
            if (response != null)
            {
                int index = response.IndexOf("pong");
                conexionStatus = index >= 0;
            }
            return conexionStatus;
        }

        internal static int GetLastBatchId(string storeId)
        {
            int batchId = 0;
            string urlPathGetLastBatchId = ConfigurationManager.AppSettings.Get(Constants.API_SYNC_ULTIMO_ID_LOTE);
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}", urlPathGetLastBatchId, storeId);
            string responseGetLastBatchId = SendHttpGetRequest(urlPath_urlQuery);
            if (responseGetLastBatchId != null && !responseGetLastBatchId.Equals("null"))
            {
                if (responseGetLastBatchId.Contains("\""))
                {
                    responseGetLastBatchId = responseGetLastBatchId.Replace("\"", "");
                }
                batchId = Convert.ToInt32(responseGetLastBatchId);
            }
            return batchId;
        }

        internal static List<Sincronizacion> GetHttpWebSynchronizations(string urlPath, string storeId, string batchId)
        {
            List<Sincronizacion> syncs = null;
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}&idLote={2}", urlPath, storeId, batchId);
            string response = SendHttpGetRequest(urlPath_urlQuery);
            if (response != null)
            {
                syncs = JsonUtils.GetListSinchronization(response);
            }

            return syncs;
        }

        internal static List<string> GetAdjustmentsTypes()
        {
            List<string> adjustmentsTypes = null;
            string urlPathGetAdjustmentsTypes = ConfigurationManager.AppSettings.Get(Constants.API_GET_TIPOS_AJUSTES);
            string responseGetAdjustmentsTypes = SendHttpGetRequest(urlPathGetAdjustmentsTypes);
            if (responseGetAdjustmentsTypes != null)
            {
                adjustmentsTypes = JsonUtils.GetListStringOfAdjustment(responseGetAdjustmentsTypes);
            }
            return adjustmentsTypes;
        }

        internal static bool HasNotInformedAndNotRuledOutReceptions(string batchId)
        {
            string urlPathHasInformedReceptions = ConfigurationManager.AppSettings.Get(Constants.API_RECEPCIONES_NOINFORMADAS_NODESCARTADAS);
            string urlPath_urlQuery = String.Format("{0}?lote={1}", urlPathHasInformedReceptions, batchId);
            string responseHasInformedReceptions = SendHttpGetRequest(urlPath_urlQuery);
            ActionResultDto actionResult = JsonUtils.GetActionResult(responseHasInformedReceptions);
            logger.Debug("Tiene recepciones no informas y no descartadas: " + actionResult.message);
            return actionResult.success;
        }

        internal static bool SearchDATsMasterFile(int activityId, string storeId, string userName)
        {
            string masterFile = ArchivosDATUtils.GetDataFileNameByIdActividad(activityId);
            string urlPathMasterFile = ConfigurationManager.AppSettings.Get(Constants.API_MAESTRO_URLPATH);
            urlPathMasterFile = String.Format(urlPathMasterFile, masterFile);
            string urlPath_urlQuery = String.Format("{0}?idSucursal={1}&perfilGenesix={2}", urlPathMasterFile, storeId, userName);
            string slashFilenameAndExtension = FileUtils.WrapSlashAndDATExtension(masterFile);
            string publicPathData = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_DATA);

            return DownloadFileFromServer(urlPath_urlQuery, slashFilenameAndExtension, publicPathData);
        }

        internal static List<VersionDispositivo> GetInfoVersions(int device, Boolean enabled)
        {
            string urlGetInfoVersions = ConfigurationManager.AppSettings.Get(Constants.API_GET_INFO_VERSION);
            string queryParams = "?dispositivo={0}&habilitada={1}";
            queryParams = String.Format(queryParams, device, enabled);
            var responseInfoVersions = HttpWebClientUtil.SendHttpGetRequest(urlGetInfoVersions + queryParams);
            if (responseInfoVersions != null)
            {
                logger.Debug("respuesta recibida de GetInfoVersiones");
                logger.Debug(responseInfoVersions);
                List<VersionDispositivo> infoVersions = JsonUtils.GetVersionDispositivo(responseInfoVersions);
                return new List<VersionDispositivo>(infoVersions);
            }
            else
            {
                return new List<VersionDispositivo>();
            }
        }

        internal static void DownloadDevicePrograms(string versionFileId, string name)
        {
            string urlDownloadFile = ConfigurationManager.AppSettings.Get(Constants.API_DOWNLOAD_PROGRAM_FILE);
            string queryParams = "?idVersionArchivo={0}";
            queryParams = String.Format(queryParams, versionFileId);
            
            string publicPathBin = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_BIN);
            DownloadFileFromServer(urlDownloadFile+queryParams, FileUtils.PrependSlash(name), publicPathBin);
        }

        internal static ActionResultDto VerifyNewBatch(string storeId)
        {
            string urlVerifyNewBatch = ConfigurationManager.AppSettings.Get(Constants.API_VERIFY_NEW_BATCH);
            string queryParams = "?idSucursal=" + storeId;
            string verifyNewBatchResponse = SendHttpGetRequest(urlVerifyNewBatch + queryParams);
            ActionResultDto actionResult = JsonUtils.GetActionResult(verifyNewBatchResponse);
            return actionResult;
        }

        internal static string GetLastVersionProgramFileFromServer(string queryParams)
        {
            string urlLastVersion = ConfigurationManager.AppSettings.Get(Constants.API_GET_LAST_VERSION_FILE_PROGRAM);
            return SendHttpGetRequest(urlLastVersion + queryParams);
        }

        #region Genesix States
        internal static string SetNoDataGenesix(string queryParams)
        {
            string urlSentGX = ConfigurationManager.AppSettings.Get("API_SET_NO_DATA_GX");
            return SendHttpGetRequest(urlSentGX + queryParams);
        }

        internal static string SetPendingGenesix(string queryParams)
        {
            string urlPendingGX = ConfigurationManager.AppSettings.Get("API_SET_PENDING_GX");
            return SendHttpGetRequest(urlPendingGX + queryParams);
        }

        internal static string SetSentGenesix(string queryParams)
        {
            string urlSentGX = ConfigurationManager.AppSettings.Get("API_SET_SENT_GX");
            return SendHttpGetRequest(urlSentGX + queryParams);
        }

        internal static string SetReceivedFromGenesix(string queryParams)
        {
            string urlReceivedGX = ConfigurationManager.AppSettings.Get("API_SET_RECEIVED_GX");
            return SendHttpGetRequest(urlReceivedGX + queryParams);
        }

        internal static string SetErrorGenesixGeneral(string queryParams)
        {
            string urlReceivedGX = ConfigurationManager.AppSettings.Get("API_SET_ERROR_GENERAL_GX");
            return SendHttpGetRequest(urlReceivedGX + queryParams);
        }

        internal static string SetUnrecoverableErrorGenesix(string queryParams)
        {
            string urlReceivedGX = ConfigurationManager.AppSettings.Get("API_SET_ERROR_DATA_GX");
            return SendHttpGetRequest(urlReceivedGX + queryParams);
        }

        internal static string SetWaitingGenesix(string queryParams)
        {
            string urlReceivedGX = ConfigurationManager.AppSettings.Get("API_SET_ERROR_DATA_GX");
            return SendHttpGetRequest(urlReceivedGX + queryParams);
        }
        #endregion

        #region Device States
        internal static string setNoDataDevice(string queryParams)
        {
            string urlNoDataFromDevice = ConfigurationManager.AppSettings.Get("API_SET_NO_DATA_PDA");
            return SendHttpGetRequest(urlNoDataFromDevice + queryParams);
        }

        internal static string SetPendingDevice(string queryParams)
        {
            string urlPendingDevice = ConfigurationManager.AppSettings.Get("API_SET_PENDING_PDA");
            return SendHttpGetRequest(urlPendingDevice + queryParams);
        }
        internal static string SetSentDeviceState(string queryParams)
        {
            string urlSentToDevice = ConfigurationManager.AppSettings.Get("API_SET_SENT_PDA");
            return SendHttpGetRequest(urlSentToDevice + queryParams);
        }

        internal static string SetReceivedDeviceState(string queryParams)
        {
            string urlReceivedFromDevice = ConfigurationManager.AppSettings.Get("API_SET_RECEIVED_PDA");
            return SendHttpGetRequest(urlReceivedFromDevice + queryParams);
        }

        internal static string SetErrorDeviceState(string queryParams)
        {
            string urlReceivedFromDevice = ConfigurationManager.AppSettings.Get("API_SET_ERROR_PDA");
            return SendHttpGetRequest(urlReceivedFromDevice + queryParams);
        }
        #endregion

        #region General States
        internal static string SetPendingGeneralState(string queryParams)
        {
            string urlPendingGeneral = ConfigurationManager.AppSettings.Get("API_SET_PENDING_GENERAL");
            return SendHttpGetRequest(urlPendingGeneral + queryParams);
        }

        internal static string SetOKGeneralState(string queryParams)
        {
            string urlOkGeneral = ConfigurationManager.AppSettings.Get("API_SET_OK_GENERAL");
            return SendHttpGetRequest(urlOkGeneral + queryParams);
        }

        internal static string SetRetryGeneralState(string queryParams)
        {
            string urlRetryGeneral = ConfigurationManager.AppSettings.Get("API_SET_RETRY_GENERAL");
            return SendHttpGetRequest(urlRetryGeneral + queryParams);
        }

        internal static string SetRetry3GeneralState(string queryParams)
        {
            string urlRetry3General = ConfigurationManager.AppSettings.Get("API_SET_RETRY3_GENERAL");
            return SendHttpGetRequest(urlRetry3General + queryParams);
        }

        internal static string SetSeeDetailsGeneralState(string queryParams)
        {
            string urlSeeDetailsGeneral = ConfigurationManager.AppSettings.Get("API_SET_SEE_DETAILS_GENERAL");
            return SendHttpGetRequest(urlSeeDetailsGeneral + queryParams);
        }

        internal static string SetModifyAdjustmentGeneralState(string queryParams)
        {
            string urlModifyAdjustmentGeneral = ConfigurationManager.AppSettings.Get("API_SET_SEE_DETAILS_GENERAL");
            return SendHttpGetRequest(urlModifyAdjustmentGeneral + queryParams);
        }

        internal static string SetPrintReceptions(string queryParams)
        {
            string urlPrintReceptionsGeneral = ConfigurationManager.AppSettings.Get("API_SET_PRINT_GENERAL");
            return SendHttpGetRequest(urlPrintReceptionsGeneral + queryParams);
        }

        internal static string SetErroPDA_General(string queryParams)
        {
            string urlErrorPDAGeneral = ConfigurationManager.AppSettings.Get("API_SET_ERROR_PDA_GENERAL");
            return SendHttpGetRequest(urlErrorPDAGeneral + queryParams);
        }

        internal static string SetErroGenesix_General(string queryParams)
        {
            string urlErrorGXGeneral = ConfigurationManager.AppSettings.Get("API_SET_ERROR_GX_GENERAL");
            return SendHttpGetRequest(urlErrorGXGeneral + queryParams);
        }
        #endregion

        internal static List<Sincronizacion> CreateNewBatch(string storeId, bool isCompleted)
        {
            string urlCreateNewBatch = ConfigurationManager.AppSettings.Get(Constants.API_CREATE_NEW_BATCH);
            string[] actionsId = new string[] { "1" };
            if(isCompleted)
            {
                actionsId = new string[] { "1", "2" };
            }
            string jsonBodyCreateNewBatch = JsonUtils.GetJsonBodyCreateNewBatch(storeId, actionsId);
            string responseCreateNewBatch = SendHttpPostRequest(urlCreateNewBatch, jsonBodyCreateNewBatch);
            List<Sincronizacion> newSync = JsonUtils.GetListSinchronization(responseCreateNewBatch);
            return newSync;
        }

        internal static string UploadFileToServer(string filePath, string queryParams)
        {
            string url = ConfigurationManager.AppSettings.Get(Constants.API_SUBIR_ARCHIVO);
            string response = SendFileHttpRequest(filePath, url + queryParams);
            return response;
        }

        internal static void ExecuteInformGenesix(long syncId, string username)
        {
            string urlInformGenesix = ConfigurationManager.AppSettings.Get(Constants.API_INFORMAR_GENESIX);
            string queryParams = "?idSincronizacion="+ new Random().Next() + "_" + syncId
                + "&perfilGenesix=" + username;
            string responseExecutedInform = SendHttpGetRequest(urlInformGenesix + queryParams);
            logger.Debug(responseExecutedInform);
        }

        internal static ActionResultDto ControlDeviceLock(long syncId, string storeId)
        {
            string urlControlBloqueoPDA = ConfigurationManager.AppSettings.Get(Constants.API_CONTROL_BLOQUEO_PDA);
            string queryParams = "?idSincronizacion=" + syncId + "&idSucursal=" + storeId;
            string responseControlBloqueoPDA = SendHttpGetRequest(urlControlBloqueoPDA + queryParams);
            return JsonUtils.GetActionResult(responseControlBloqueoPDA);
        }

        internal static ListView LoadAdjustmentsGrid(string lote, string estadoInformado,
            string page = "1", string rows = "20", string sidx = "lote", string sord = "asc")
        {
            string urlModifyLoadAdjustmentsGrid = ConfigurationManager.AppSettings.Get(Constants.API_CARGA_GRILLA_AJUSTES);
            string queryParams = "?lote="+lote+"&page="+page+"&rows="+page+"&sidx="+sidx+"&sord="+sord;
            string responseModifyLoadAdjustmentsGrid = SendHttpGetRequest(urlModifyLoadAdjustmentsGrid + queryParams);
            return JsonUtils.GetAjustesDTO(responseModifyLoadAdjustmentsGrid);
        }

        internal static ListView LoadReceptionsGrid(long lote = 111152, string informedReceptions = "false", 
            string page = "1", string rows = "20", string sord = "desc", string sidx = "idRecepcion")
        {
            string urlSeeDetailsReceptionsGrid = ConfigurationManager.AppSettings.Get(Constants.API_CARGA_GRILLA_RECEPCIONES);
            Int32 nd = DateTimeUtils.GetUnixTimeFromUTCNow();
            string queryParams = "?recepcionesInformadas=" + informedReceptions + "&lote=" + lote.ToString() +
                "&page=" + page + "&rows=" + rows + "&sord=" +sord + "&sidx=" + sidx + "&nd=" + nd;
            string responseModifyLoadAdjustmentsGrid = SendHttpGetRequest(urlSeeDetailsReceptionsGrid + queryParams);
            return JsonUtils.GetRecepciones(responseModifyLoadAdjustmentsGrid);
        }

        internal static ActionResultDto DeleteAdjustment(long adjustmentId, string batchId, long syncId)
        {
            string urlDeleteAdjustmentModify = ConfigurationManager.AppSettings.Get(Constants.API_MODIFICAR_ELIMINAR_AJUTE);
            string queryParams = "?idAjustes={0}&lote={1}&idSincronizacion={2}";
            queryParams = String.Format(queryParams, adjustmentId, batchId, syncId);
            string responseDeleteAdjustment = SendHttpGetRequest(urlDeleteAdjustmentModify + queryParams);
            return JsonUtils.GetActionResult(responseDeleteAdjustment);
        }

        internal static string UpdateModifiedAdjustments(string batchId, ObservableCollection<Ajustes> adjustments, long syncId)
        {
            string urlUpdateModifiedAdjustments = ConfigurationManager.AppSettings.Get(Constants.API_MODIFICAR_ACTUALIZAR_AJUSTES);
            string jsonBodyModifiedAdjustments = JsonUtils.GetJsonBodyModifyAdjustments(adjustments, syncId, batchId);
            logger.Debug("UpdateModifiedAdjustments");
            logger.Debug("cuerpo del delito: " + jsonBodyModifiedAdjustments);
            string responseUpdateModifyAdjustments = SendHttpPostRequest(urlUpdateModifiedAdjustments, jsonBodyModifiedAdjustments);
            return responseUpdateModifyAdjustments;
        }


        internal static Dictionary<string, string> DiscardReceptions(string batchId, string syncId)
        {
            string urlDiscardReceptions = ConfigurationManager.AppSettings.Get(Constants.API_MODIFICAR_DESCARTAR_RECEPCIONES);
            string queryParams = "?lote=" + batchId + "&idSincronizacion=" + syncId;
            string responseDiscardReceptions = SendHttpGetRequest(urlDiscardReceptions + queryParams);
            Dictionary<string, string> discardReceptionDictionary = JsonUtils.GetDiscardReception(responseDiscardReceptions);
            return discardReceptionDictionary;
        }

        internal static ListView GetReceptionGrid(string batchId, string informedState,
            string page = "1", string rows = "10", string sidx = "id", string sord = "asc")
        {
            string urlReceptionGrid = ConfigurationManager.AppSettings.Get(Constants.API_CARGA_GRILLA_RECEPCIONES);
            string queryParams = "?idLote=" + batchId + "&estadoInformado=" + informedState 
                + "&page=" + page  +"&rows=" + rows + "&sidx=" + sidx + "&sord=" + sord;
            string responseReceptionGrid = SendHttpGetRequest(urlReceptionGrid + queryParams);
            return JsonUtils.GetListView(responseReceptionGrid);
        }

        internal static string PrintReception(string receptionNumber, string storeId)
        {
            string urlPath = ConfigurationManager.AppSettings.Get(Constants.API_INFORMES_REMITO_PDF);
            string tempFileRemito = ConfigurationManager.AppSettings.Get(Constants.TEMP_FILE_REMITO);
            string queryParams = "?numeroRecepcion=" + receptionNumber + "&codigoTienda=" + storeId;
            string slashFilenameAndExtension = FileUtils.GetTempFileName(tempFileRemito);
            string destiny = ConfigurationManager.AppSettings.Get(Constants.PUBLIC_PATH_TEMP);
            string destinyExtended = TextUtils.ExpandEnviromentVariable(destiny);
            FileUtils.VerifyFoldersOrCreate(destinyExtended);
            DownloadFileFromServer(urlPath + queryParams, slashFilenameAndExtension, destinyExtended);

            return destinyExtended + slashFilenameAndExtension;
        }

        internal static ListView GetAdjustmentsByBatchId(string batchId, string rows = "10", string page = "1")
        {
            string urlGetAdjustmentsByBatchId = ConfigurationManager.AppSettings.Get(Constants.API_GET_ADJUSTMENTS_BY_BATCH_ID);
            Int32 nd = DateTimeUtils.GetUnixTimeFromUTCNow();
            string queryParams = "?lote="+ batchId + "&_search=false"
                +"&nd="+nd+"&rows="+rows+"&page="+page+"&sidx=idAjuste&sord=asc";
            string responseGetAdjustmentsByBatchId = SendHttpGetRequest(urlGetAdjustmentsByBatchId + queryParams);
            return JsonUtils.GetListView(responseGetAdjustmentsByBatchId);
        }

        internal static string SearchBatches(string idSucursal, int page = 1, long rows = 20)
        {
            string urlSearchBatches = ConfigurationManager.AppSettings.Get(Constants.API_CARGA_GRILLA_LOTES);
            Int32 nd = DateTimeUtils.GetUnixTimeFromUTCNow();
            string queryParams = "?idSucursal="+ idSucursal + "&_search=false&nd="+ nd 
                + "&rows=" + rows + "&page=" + page + "&sidx=idLote&sord=desc";
            string responseSearchBatches = SendHttpGetRequest(urlSearchBatches + queryParams);
            return responseSearchBatches;
        }

        internal static UserKey AttemptAuthenticatePortalImagoSur(string username, string password)
        {
            string urlPath = ConfigurationManager.AppSettings.Get(Constants.PORTAL_AUTHENTICATE);
            string jsonBody = JsonUtils.GetJsonBodyUser(username, password);
            string urlAuthority = ConfigurationManager.AppSettings.Get(Constants.PORTAL_SERVER_HOST);
            string userKeyResponse = SendHttpPostRequest(urlPath, jsonBody, urlAuthority);
            return JsonUtils.GetUserKey(userKeyResponse);
        }

    }
}

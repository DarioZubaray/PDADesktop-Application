using log4net;
using System.Configuration;
using System;

namespace PDADesktop.Classes.Utils
{
    public class SynchronizationStateUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Genesix State
        internal static void SetNoDataGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetNoDataGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetPendingGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetPendingGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetSentGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSentGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetReceivedFromGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetReceivedFromGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetErrorGenesixGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErrorGenesixGeneral(queryParams);
            logger.Debug(response);
        }

        internal static void SetUnrecoverableErrorGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetUnrecoverableErrorGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetWaitingGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetWaitingGenesix(queryParams);
            logger.Debug(response);
        }
        #endregion

        #region DeviceState
        internal static void SetNoDataDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.setNoDataDevice(queryParams);
            logger.Debug(queryParams);
        }

        internal static void SetPendingDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetPendingDevice(queryParams);
            logger.Debug(queryParams);
        }

        internal static void SetSentDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSentDeviceState(queryParams);
            logger.Debug(response);
        }

        internal static void SetReceivedDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetReceivedDeviceState(queryParams);
            logger.Debug(response);
        }

        internal static void SetErrorDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErrorDeviceState(queryParams);
            logger.Debug(response);
        }
        #endregion

        #region General State
        internal static void SetPendingGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetPendingGeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetOKGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetOKGeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetRetryGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetRetryGeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetRetry3GeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetRetry3GeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetSeeDetailsGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSeeDetailsGeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetModifyAdjustmentGeneralState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetModifyAdjustmentGeneralState(queryParams);
            logger.Debug(response);
        }

        internal static void SetPrintReceptions(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetPrintReceptions(queryParams);
            logger.Debug(response);
        }

        internal static void SetErroPDA_General(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErroPDA_General(queryParams);
            logger.Debug(response);
        }

        internal static void SetErroGenesix_General(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErroGenesix_General(queryParams);
            logger.Debug(response);
        }
        #endregion
    }
}

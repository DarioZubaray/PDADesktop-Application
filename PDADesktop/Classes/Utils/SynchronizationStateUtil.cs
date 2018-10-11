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
            string response = HttpWebClientUtil.SetNoDataGenesixState(queryParams);
            logger.Debug(response);
        }

        internal static void SetSentGenesixState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSentGenesixState(queryParams);
            logger.Debug(response);
        }

        internal static void SetReceivedFromGenesix(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetReceivedFromGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetErrorGenesixGeneral(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErrorGenesixGeneral(queryParams);
            logger.Debug(response);
        }
        #endregion

        #region DeviceState
        internal static void SetReceivedDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetReceivedDeviceState(queryParams);
            logger.Debug(response);
        }

        internal static void SetSentDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSentDeviceState(queryParams);
            logger.Debug(response);
        }

        internal static void SetNoDataDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.setNoDataDeviceState(queryParams);
            logger.Debug(queryParams);
        }

        internal static void SetErrorDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErrorDeviceState(queryParams);
            logger.Debug(queryParams);
        }
        #endregion

        #region General State
        internal static void SetOKStateGeneral(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetOKStateGeneral(queryParams);
            logger.Debug(response);
        }
        #endregion
    }
}

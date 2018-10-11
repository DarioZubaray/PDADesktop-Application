using log4net;
using System.Configuration;
using System;

namespace PDADesktop.Classes.Utils
{
    public class SynchronizationStateUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Genesix State
        internal static void SetSentGenesixState(string syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetSentGenesixState(queryParams);
            logger.Debug(response);
        }

        internal static void SetReceivedFromGenesix(string syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetReceivedFromGenesix(queryParams);
            logger.Debug(response);
        }

        internal static void SetErrorGenesixGeneral(string syncId)
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

        internal static void SetNoDataDeviceState(long syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.setNoDataDeviceState(queryParams);
            logger.Debug(queryParams);
        }

        internal static void SetErrorDeviceState(object syncId)
        {
            string queryParams = "?idSincronizacion=" + syncId;
            string response = HttpWebClientUtil.SetErrorDeviceState(queryParams);
            logger.Debug(queryParams);
        }
        #endregion
    }
}

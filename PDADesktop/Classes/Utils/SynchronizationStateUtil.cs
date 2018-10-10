using log4net;
using System.Configuration;

namespace PDADesktop.Classes.Utils
{
    public class SynchronizationStateUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetSentGenesixState(string synchronizationId)
        {
            string queryParams = "?idSincronizacion=" + synchronizationId;
            string response = HttpWebClientUtil.SetSentGenesixState(queryParams);
            logger.Debug(response);
        }

        public static void SetReceivedDeviceState(string synchronizationId)
        {
            string queryParams = "?idSincronizacion=" + synchronizationId;
            string response = HttpWebClientUtil.SetReceivedDeviceState(queryParams);
            logger.Debug(response);
        }

    }
}

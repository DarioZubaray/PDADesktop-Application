using log4net;
using System.Configuration;

namespace PDADesktop.Classes.Utils
{
    public class SynchronizationStateUtil
    {
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void SetSentGenesixState(string synchronizationId)
        {
            string urlSentGX = ConfigurationManager.AppSettings.Get("API_SET_SENT_GX");
            string queryParams = "?idSincronizacion=" + synchronizationId;
            string response = HttpWebClientUtil.SendHttpGetRequest(urlSentGX + queryParams);
            logger.Debug(response);
        }

        public static void SetReceivedDeviceState(string synchronizationId)
        {
            string urlReceivedFromDevice = ConfigurationManager.AppSettings.Get("API_SET_RECEIVED_PDA");
            string queryParams = "?idSincronizacion=" + synchronizationId;
            string response = HttpWebClientUtil.SendHttpGetRequest(urlReceivedFromDevice + queryParams);
            logger.Debug(response);
        }

    }
}

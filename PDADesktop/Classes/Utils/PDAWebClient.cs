using log4net;
using System;
using System.Net;

namespace PDADesktop.Classes.Utils
{
    public class PDAWebClient : WebClient
    {
        #region Attributes
        private int timeout { get; set; }
        private string url { get; set; }
        private int retries { get; set; }

        public const string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36";
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public PDAWebClient()
        {
            this.timeout = 600000;
            this.retries = 0;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
        }
        public PDAWebClient(string url, int timeout = 600000)
        {
            this.timeout = timeout;
            this.retries = 0;
            this.Headers.Add("user-agent", DEFAULT_USER_AGENT);
        }
        #endregion

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest webRequest = base.GetWebRequest(uri);
            logger.Info("override: Seteando timeout a " + timeout);
            webRequest.Timeout = timeout;
            ((HttpWebRequest)webRequest).ReadWriteTimeout = timeout;

            return webRequest;
        }

        public string GetRequest(string url, int timeout = 600000, int retries = 3, string userAgent = DEFAULT_USER_AGENT)
        {
            var lWebClient = new PDAWebClient();
            try
            {
                logger.Info("GetRequest: Seteando timeout a " + timeout);
                lWebClient.timeout = timeout;
                lWebClient.Headers.Add("user-agent", userAgent);
                lWebClient.url = url;
                lWebClient.retries = retries;
                return lWebClient.DownloadString(url);
            }
            catch
            {
                Console.WriteLine("Cacheando el timeout, reintentado...");
                return lWebClient.Retry();
            }
            finally
            {
                lWebClient.Dispose();
            }
        }

        private string Retry()
        {
            string retryResponse = "";
            int retryCount = 0;
            bool stopRetry = false;
            while (!stopRetry)
            {
                retryCount++;
                if (retryCount > this.retries)
                {
                    logger.Info("Se agotaron todos los reintentos");
                    stopRetry = true;
                    retryResponse = null;
                    break;
                }
                try
                {
                    logger.Info("Reintentando conectar a: " + this.url);
                    logger.Debug("Reintento numero: " + retryCount);
                    retryResponse = this.DownloadString(this.url);
                }
                catch
                {
                    logger.Info("Se excedio el tiempo de espera para el reintento: " + retryCount);
                }
            }
            return retryResponse;
        }
    }
}

